using System.Collections.Generic;

namespace TurnBasedSystem;

/// <summary>
/// 单阶段 — 象棋/Initiative/ATB/Tick
/// </summary>
public class SinglePhase : ITurnPhaseSequence
{
    private static readonly List<TurnPhaseConfig> Phases = new()
    {
        new TurnPhaseConfig("Main", AllowActions: true, IsAutomatic: false)
    };

    public IReadOnlyList<TurnPhaseConfig> GetPhases() => Phases;
}
