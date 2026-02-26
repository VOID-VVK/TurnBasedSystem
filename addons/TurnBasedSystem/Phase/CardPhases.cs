using System.Collections.Generic;

namespace TurnBasedSystem;

/// <summary>
/// 卡牌阶段 — 抽牌/主阶段/弃牌/结束
/// </summary>
public class CardPhases : ITurnPhaseSequence
{
    private static readonly List<TurnPhaseConfig> Phases = new()
    {
        new TurnPhaseConfig("Draw", AllowActions: false, IsAutomatic: true),
        new TurnPhaseConfig("Main", AllowActions: true, IsAutomatic: false),
        new TurnPhaseConfig("Discard", AllowActions: false, IsAutomatic: true),
        new TurnPhaseConfig("End", AllowActions: false, IsAutomatic: true)
    };

    public IReadOnlyList<TurnPhaseConfig> GetPhases() => Phases;
}
