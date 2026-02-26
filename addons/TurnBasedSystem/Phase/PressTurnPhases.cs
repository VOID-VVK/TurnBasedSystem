using System.Collections.Generic;

namespace TurnBasedSystem;

/// <summary>
/// Press Turn 阶段 — 带图标管理的主阶段
/// </summary>
public class PressTurnPhases : ITurnPhaseSequence
{
    private static readonly List<TurnPhaseConfig> Phases = new()
    {
        new TurnPhaseConfig("PressTurn", AllowActions: true, IsAutomatic: false)
    };

    public IReadOnlyList<TurnPhaseConfig> GetPhases() => Phases;
}
