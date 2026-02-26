using System.Collections.Generic;

namespace TurnBasedSystem;

/// <summary>
/// Press Turn 图标预算 — 女神异闻录
/// </summary>
public class PressTurnIcons : IActionBudget
{
    private readonly int _iconsPerTurn;
    private readonly Dictionary<string, float> _icons = new();

    public PressTurnIcons(int iconsPerTurn = 4)
    {
        _iconsPerTurn = iconsPerTurn;
    }

    public void RefillBudget(ITurnUnit unit) => _icons[unit.UnitId] = _iconsPerTurn;

    public bool CanAct(ITurnUnit unit) =>
        _icons.TryGetValue(unit.UnitId, out var icons) && icons >= 0.5f;

    public void Spend(ITurnUnit unit, int cost)
    {
        if (_icons.ContainsKey(unit.UnitId))
            _icons[unit.UnitId] -= cost;
    }

    public void GrantExtra(ITurnUnit unit, int amount)
    {
        // 弱点命中：消耗半个图标而非整个
        if (_icons.ContainsKey(unit.UnitId))
            _icons[unit.UnitId] += 0.5f;
    }

    public float GetRemainingIcons(string unitId) =>
        _icons.TryGetValue(unitId, out var icons) ? icons : 0;
}
