using System.Collections.Generic;

namespace TurnBasedSystem;

/// <summary>
/// 单次行动预算 — 每回合 1 次行动
/// </summary>
public class SingleAction : IActionBudget
{
    private readonly HashSet<string> _acted = new();

    public void RefillBudget(ITurnUnit unit) => _acted.Remove(unit.UnitId);
    public bool CanAct(ITurnUnit unit) => !_acted.Contains(unit.UnitId);
    public void Spend(ITurnUnit unit, int cost) => _acted.Add(unit.UnitId);
    public void GrantExtra(ITurnUnit unit, int amount) => _acted.Remove(unit.UnitId);
}
