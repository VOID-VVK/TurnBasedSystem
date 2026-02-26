namespace TurnBasedSystem;

/// <summary>
/// 下一步行动的描述
/// </summary>
public class TurnStep
{
    /// <summary>需要行动的单位（单位级行动时非 null）</summary>
    public ITurnUnit? Unit { get; init; }

    /// <summary>需要行动的阵营（阵营级行动时非 null）</summary>
    public ITurnFaction? Faction { get; init; }

    /// <summary>是否为同时行动（所有方同时规划）</summary>
    public bool IsSimultaneous { get; init; }
}
