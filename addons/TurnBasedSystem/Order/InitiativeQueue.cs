using System.Collections.Generic;
using System.Linq;

namespace TurnBasedSystem;

/// <summary>
/// 速度先攻制 — 博德之门3/FFX CTB
/// </summary>
public class InitiativeQueue : ITurnOrderResolver
{
    private readonly bool _recalcAfterAction;
    private Queue<ITurnUnit> _queue = new();
    private bool _acted;

    public InitiativeQueue(bool recalcAfterAction = false)
    {
        _recalcAfterAction = recalcAfterAction;
    }

    public TurnStep? GetNext(TurnContext ctx)
    {
        if (_acted && !_recalcAfterAction) return null;
        if (_queue.Count == 0) return null;

        _acted = true;
        var unit = _queue.Dequeue();
        return new TurnStep { Unit = unit };
    }

    public void OnRoundStart(TurnContext ctx)
    {
        _acted = false;
        RebuildQueue(ctx);
    }

    public void OnActionResolved(TurnContext ctx, ActionResult result)
    {
        if (_recalcAfterAction)
            _acted = false;
    }

    private void RebuildQueue(TurnContext ctx)
    {
        var allUnits = ctx.Factions
            .SelectMany(f => f.GetActiveUnits())
            .OrderByDescending(u => u.Initiative);
        _queue = new Queue<ITurnUnit>(allUnits);
    }

    public void Reset()
    {
        _queue.Clear();
        _acted = false;
    }
}
