using System.Collections.Generic;
using System.Linq;

namespace TurnBasedSystem;

/// <summary>
/// Roguelike 滴答制 — 玩家动一步，所有敌人各动一步
/// </summary>
public class TickOrder : ITurnOrderResolver
{
    private Queue<ITurnUnit> _queue = new();

    public TurnStep? GetNext(TurnContext ctx)
    {
        if (_queue.Count == 0) return null;
        return new TurnStep { Unit = _queue.Dequeue() };
    }

    public void OnRoundStart(TurnContext ctx)
    {
        // 玩家先行动，然后所有非玩家单位
        var units = new List<ITurnUnit>();
        foreach (var faction in ctx.Factions.OrderBy(f => f.IsPlayerControlled ? 0 : 1))
            units.AddRange(faction.GetActiveUnits());
        _queue = new Queue<ITurnUnit>(units);
    }

    public void OnActionResolved(TurnContext ctx, ActionResult result) { }

    public void Reset()
    {
        _queue.Clear();
    }
}
