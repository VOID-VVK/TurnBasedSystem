using System.Collections.Generic;

namespace TurnBasedSystem;

/// <summary>
/// ATB gauge 预算 — gauge 满 = 1 次行动，行动后清零
/// </summary>
public class GaugeBased : IActionBudget
{
    private readonly HashSet<string> _acted = new();

    public void RefillBudget(ITurnUnit unit) => _acted.Remove(unit.UnitId);
    public bool CanAct(ITurnUnit unit) => !_acted.Contains(unit.UnitId) && unit.ATBGauge >= 1.0f;

    public void Spend(ITurnUnit unit, int cost)
    {
        unit.ATBGauge = 0f;
        _acted.Add(unit.UnitId);
    }

    public void GrantExtra(ITurnUnit unit, int amount) => _acted.Remove(unit.UnitId);
}
