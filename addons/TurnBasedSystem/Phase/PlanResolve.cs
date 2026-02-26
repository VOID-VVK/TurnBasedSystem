using System.Collections.Generic;

namespace TurnBasedSystem;

/// <summary>
/// 规划-结算阶段 — 同时行动制
/// </summary>
public class PlanResolve : ITurnPhaseSequence
{
    private static readonly List<TurnPhaseConfig> Phases = new()
    {
        new TurnPhaseConfig("Planning", AllowActions: true, IsAutomatic: false),
        new TurnPhaseConfig("Resolution", AllowActions: false, IsAutomatic: true)
    };

    public IReadOnlyList<TurnPhaseConfig> GetPhases() => Phases;
}
