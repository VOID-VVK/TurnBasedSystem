using System.Collections.Generic;

namespace TurnBasedSystem;

/// <summary>
/// 只读上下文快照 — 传递给各策略组件
/// </summary>
public class TurnContext
{
    public int RoundNumber { get; set; }
    public string CurrentPhaseName { get; set; } = "";
    public IReadOnlyList<ITurnFaction> Factions { get; set; } = new List<ITurnFaction>();
    public ITurnFaction? ActiveFaction { get; set; }
    public ITurnUnit? ActiveUnit { get; set; }
}
