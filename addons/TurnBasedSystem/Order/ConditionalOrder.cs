using System.Collections.Generic;
using System.Linq;

namespace TurnBasedSystem;

/// <summary>
/// 弱点连锁制 — 女神异闻录/真女神转生
/// </summary>
public class ConditionalOrder : ITurnOrderResolver
{
    private Queue<ITurnUnit> _queue = new();
    private bool _grantedExtra;

    public TurnStep? GetNext(TurnContext ctx)
    {
        if (_queue.Count == 0) return null;
        _grantedExtra = false;
        return new TurnStep { Unit = _queue.Dequeue() };
    }

    public void OnRoundStart(TurnContext ctx)
    {
        var allUnits = ctx.Factions
            .SelectMany(f => f.GetActiveUnits())
            .OrderByDescending(u => u.Initiative);
        _queue = new Queue<ITurnUnit>(allUnits);
    }

    public void OnActionResolved(TurnContext ctx, ActionResult result)
    {
        if (result == ActionResult.Critical && ctx.ActiveUnit != null && !_grantedExtra)
        {
            // 弱点命中：额外行动
            var list = new List<ITurnUnit>(_queue);
            list.Insert(0, ctx.ActiveUnit);
            _queue = new Queue<ITurnUnit>(list);
            _grantedExtra = true;
        }
    }

    public void Reset()
    {
        _queue.Clear();
        _grantedExtra = false;
    }
}
