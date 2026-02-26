using System.Collections.Generic;

namespace TurnBasedSystem;

/// <summary>
/// 维度4: 阶段序列
/// </summary>
public interface ITurnPhaseSequence
{
    IReadOnlyList<TurnPhaseConfig> GetPhases();
}
