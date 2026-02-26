using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TurnBasedSystem;

/// <summary>
/// 回合制编排器 — Godot Node，驱动整个回合循环
/// </summary>
public partial class TurnSystemNode : Node
{
    // ==================== 信号 ====================
    [Signal] public delegate void RoundStartedEventHandler(int roundNumber);
    [Signal] public delegate void RoundEndedEventHandler(int roundNumber);
    [Signal] public delegate void PhaseChangedEventHandler(string phaseName);
    [Signal] public delegate void UnitTurnStartedEventHandler(string unitId);
    [Signal] public delegate void UnitTurnEndedEventHandler(string unitId);
    [Signal] public delegate void FactionTurnStartedEventHandler(string factionId);
    [Signal] public delegate void FactionTurnEndedEventHandler(string factionId);
    [Signal] public delegate void ActionResolvedEventHandler(string unitId, string actionName, int result);
    [Signal] public delegate void TimelineChangedEventHandler();
    [Signal] public delegate void BudgetChangedEventHandler(string unitId, int remaining);

    // ==================== 四维策略 ====================
    private ITurnOrderResolver _orderResolver = null!;
    private IActionBudget _actionBudget = null!;
    private IResolutionPolicy _resolutionPolicy = null!;
    private ITurnPhaseSequence _phaseSequence = null!;

    // ==================== 状态 ====================
    private readonly List<ITurnFaction> _factions = new();
    private TurnContext _ctx = new();
    private bool _running;

    // ==================== 回调 ====================
    public Func<TurnContext, TurnStep, Task<ITurnAction?>>? OnPlayerInput { get; set; }
    public Func<TurnContext, TurnStep, Task<List<ITurnAction>>>? OnAIDecision { get; set; }
    public Func<ITurnAction, ActionResult, Task>? OnAnimate { get; set; }
    /// <summary>批量动画回调 — 用于阵营级批量行动的动画播放</summary>
    public Func<List<(ITurnAction action, ActionResult result)>, Task>? OnAnimateBatch { get; set; }
    public Func<TurnContext, Task>? OnPhaseEnter { get; set; }
    public Func<TurnContext, Task>? OnPhaseExit { get; set; }

    public TurnContext Context => _ctx;
    public bool IsRunning => _running;

    public void Configure(TurnSystemConfig config)
    {
        _orderResolver = config.OrderResolver;
        _actionBudget = config.ActionBudget;
        _resolutionPolicy = config.ResolutionPolicy;
        _phaseSequence = config.PhaseSequence;
    }

    public void RegisterFaction(ITurnFaction faction)
    {
        _factions.Add(faction);
    }

    /// <summary>
    /// 执行一个完整回合
    /// </summary>
    public async Task ExecuteRound()
    {
        if (_running) return;
        _running = true;

        _ctx.RoundNumber++;
        _ctx.Factions = _factions.AsReadOnly();
        _orderResolver.OnRoundStart(_ctx);
        EmitSignal(SignalName.RoundStarted, _ctx.RoundNumber);

        var phases = _phaseSequence.GetPhases();
        foreach (var phase in phases)
        {
            _ctx.CurrentPhaseName = phase.PhaseName;
            EmitSignal(SignalName.PhaseChanged, phase.PhaseName);
            if (OnPhaseEnter != null) await OnPhaseEnter(_ctx);

            if (phase.IsAutomatic)
            {
                if (OnPhaseExit != null) await OnPhaseExit(_ctx);
                continue;
            }

            if (phase.AllowActions)
                await ExecutePhaseActions();

            if (OnPhaseExit != null) await OnPhaseExit(_ctx);
        }

        EmitSignal(SignalName.RoundEnded, _ctx.RoundNumber);
        _running = false;
    }

    private async Task ExecutePhaseActions()
    {
        while (true)
        {
            var step = _orderResolver.GetNext(_ctx);
            if (step == null) break;

            if (step.Unit != null)
                await ExecuteUnitStep(step);
            else if (step.Faction != null)
                await ExecuteFactionStep(step);
            else if (step.IsSimultaneous)
                await ExecuteSimultaneousStep(step);
        }
    }

    private async Task ExecuteUnitStep(TurnStep step)
    {
        var unit = step.Unit!;
        _ctx.ActiveUnit = unit;
        _actionBudget.RefillBudget(unit);
        unit.OnTurnStart();
        EmitSignal(SignalName.UnitTurnStarted, unit.UnitId);

        while (_actionBudget.CanAct(unit))
        {
            var action = await GetAction(step);
            if (action == null) break;

            var result = await _resolutionPolicy.Submit(action, _ctx);
            _actionBudget.Spend(unit, action.Cost);
            _orderResolver.OnActionResolved(_ctx, result);
            EmitSignal(SignalName.ActionResolved, unit.UnitId, action.ActionName, (int)result);
            if (OnAnimate != null) await OnAnimate(action, result);
        }

        unit.OnTurnEnd();
        EmitSignal(SignalName.UnitTurnEnded, unit.UnitId);
        _ctx.ActiveUnit = null;
    }

    private async Task ExecuteFactionStep(TurnStep step)
    {
        var faction = step.Faction!;
        _ctx.ActiveFaction = faction;
        EmitSignal(SignalName.FactionTurnStarted, faction.FactionId);

        foreach (var unit in faction.GetActiveUnits())
            _actionBudget.RefillBudget(unit);

        if (faction.IsPlayerControlled && OnPlayerInput != null)
        {
            // 玩家阵营：逐单位获取输入
            foreach (var unit in faction.GetActiveUnits())
            {
                _ctx.ActiveUnit = unit;
                unit.OnTurnStart();
                var action = await OnPlayerInput(_ctx, step);
                if (action != null)
                {
                    var result = await _resolutionPolicy.Submit(action, _ctx);
                    _actionBudget.Spend(unit, action.Cost);
                    _orderResolver.OnActionResolved(_ctx, result);
                    EmitSignal(SignalName.ActionResolved, unit.UnitId, action.ActionName, (int)result);
                    if (OnAnimate != null) await OnAnimate(action, result);
                }
                unit.OnTurnEnd();
            }
        }
        else if (OnAIDecision != null)
        {
            // AI 阵营：批量获取决策
            var actions = await OnAIDecision(_ctx, step);
            if (actions.Count > 0)
            {
                // 先执行所有行动的数据变更
                var results = new List<(ITurnAction action, ActionResult result)>();
                foreach (var action in actions)
                {
                    var result = await _resolutionPolicy.Submit(action, _ctx);
                    results.Add((action, result));
                }
                await _resolutionPolicy.ResolveAll(_ctx);

                // 批量动画
                if (OnAnimateBatch != null)
                {
                    await OnAnimateBatch(results);
                }
                else if (OnAnimate != null)
                {
                    foreach (var (action, result) in results)
                        await OnAnimate(action, result);
                }
            }
        }

        EmitSignal(SignalName.FactionTurnEnded, faction.FactionId);
        _ctx.ActiveFaction = null;
    }

    private async Task ExecuteSimultaneousStep(TurnStep step)
    {
        // 收集所有阵营的计划
        foreach (var faction in _factions)
        {
            _ctx.ActiveFaction = faction;
            if (faction.IsPlayerControlled && OnPlayerInput != null)
            {
                var action = await OnPlayerInput(_ctx, step);
                if (action != null)
                    await _resolutionPolicy.Submit(action, _ctx);
            }
            else if (OnAIDecision != null)
            {
                var actions = await OnAIDecision(_ctx, step);
                foreach (var action in actions)
                    await _resolutionPolicy.Submit(action, _ctx);
            }
        }

        await _resolutionPolicy.ResolveAll(_ctx);
        _ctx.ActiveFaction = null;
    }

    private async Task<ITurnAction?> GetAction(TurnStep step)
    {
        var unit = step.Unit!;
        var faction = _factions.Find(f => f.FactionId == unit.FactionId);
        if (faction == null) return null;

        if (faction.IsPlayerControlled && OnPlayerInput != null)
            return await OnPlayerInput(_ctx, step);

        if (OnAIDecision != null)
        {
            var actions = await OnAIDecision(_ctx, step);
            return actions.Count > 0 ? actions[0] : null;
        }

        return null;
    }

    public void Reset()
    {
        _ctx = new TurnContext();
        _factions.Clear();
        _orderResolver?.Reset();
        _running = false;
    }
}
