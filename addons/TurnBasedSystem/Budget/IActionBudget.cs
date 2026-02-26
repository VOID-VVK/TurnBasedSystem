namespace TurnBasedSystem;

/// <summary>
/// 维度2: 行动预算
/// </summary>
public interface IActionBudget
{
    void RefillBudget(ITurnUnit unit);
    bool CanAct(ITurnUnit unit);
    void Spend(ITurnUnit unit, int cost);
    void GrantExtra(ITurnUnit unit, int amount);
}
