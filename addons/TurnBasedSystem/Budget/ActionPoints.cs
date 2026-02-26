using System.Collections.Generic;

namespace TurnBasedSystem;

/// <summary>
/// 行动点数预算 — 杀戮尖塔/神界原罪2
/// </summary>
public class ActionPoints : IActionBudget
{
    private readonly int _defaultAP;
    private readonly Dictionary<string, int> _remaining = new();

    public ActionPoints(int defaultAP = 3)
    {
        _defaultAP = defaultAP;
    }

    public void RefillBudget(ITurnUnit unit) => _remaining[unit.UnitId] = _defaultAP;

    public bool CanAct(ITurnUnit unit) =>
        _remaining.TryGetValue(unit.UnitId, out var ap) && ap > 0;

    public void Spend(ITurnUnit unit, int cost)
    {
        if (_remaining.ContainsKey(unit.UnitId))
            _remaining[unit.UnitId] -= cost;
    }

    public void GrantExtra(ITurnUnit unit, int amount)
    {
        if (_remaining.ContainsKey(unit.UnitId))
            _remaining[unit.UnitId] += amount;
    }

    public int GetRemaining(string unitId) =>
        _remaining.TryGetValue(unitId, out var ap) ? ap : 0;
}
