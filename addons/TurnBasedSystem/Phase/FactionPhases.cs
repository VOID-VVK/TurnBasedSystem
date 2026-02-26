using System.Collections.Generic;

namespace TurnBasedSystem;

/// <summary>
/// 阵营阶段 — 每个阵营一个阶段
/// </summary>
public class FactionPhases : ITurnPhaseSequence
{
    private readonly List<TurnPhaseConfig> _phases;

    public FactionPhases(int factionCount)
    {
        _phases = new List<TurnPhaseConfig>();
        for (int i = 0; i < factionCount; i++)
            _phases.Add(new TurnPhaseConfig($"Faction_{i}", AllowActions: true, IsAutomatic: false));
    }

    public IReadOnlyList<TurnPhaseConfig> GetPhases() => _phases;
}
