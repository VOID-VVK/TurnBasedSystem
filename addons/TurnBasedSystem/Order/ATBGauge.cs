using System.Collections.Generic;
using System.Linq;

namespace TurnBasedSystem;

/// <summary>
/// ATB 半即时制 — FF4~FF9
/// </summary>
public class ATBGauge : ITurnOrderResolver
{
    private readonly float _gaugeSpeed;
    private ITurnUnit? _readyUnit;

    public ATBGauge(float gaugeSpeed = 1.0f)
    {
        _gaugeSpeed = gaugeSpeed;
    }

    public TurnStep? GetNext(TurnContext ctx)
    {
        if (_readyUnit != null)
        {
            var step = new TurnStep { Unit = _readyUnit };
            _readyUnit = null;
            return step;
        }

        // 累加所有单位的 gauge，找到第一个满的
        var allUnits = ctx.Factions
            .SelectMany(f => f.GetActiveUnits())
            .Where(u => u.IsActive)
            .ToList();

        foreach (var unit in allUnits)
        {
            unit.ATBGauge += _gaugeSpeed * unit.Initiative * 0.01f;
            if (unit.ATBGauge >= 1.0f)
            {
                _readyUnit = unit;
                break;
            }
        }

        if (_readyUnit != null)
        {
            var step = new TurnStep { Unit = _readyUnit };
            _readyUnit = null;
            return step;
        }

        return null;
    }

    public void OnRoundStart(TurnContext ctx) { }

    public void OnActionResolved(TurnContext ctx, ActionResult result)
    {
        if (ctx.ActiveUnit != null)
            ctx.ActiveUnit.ATBGauge = 0f;
    }

    public void Reset()
    {
        _readyUnit = null;
    }
}
