using System.Collections.Generic;

namespace TurnBasedSystem;

/// <summary>
/// 阵营接口
/// </summary>
public interface ITurnFaction
{
    string FactionId { get; }
    int Order { get; }
    bool IsPlayerControlled { get; }
    IReadOnlyList<ITurnUnit> GetActiveUnits();
}
