namespace TurnBasedSystem;

/// <summary>
/// 回合阶段配置
/// </summary>
public record TurnPhaseConfig(
    string PhaseName,
    bool AllowActions,
    bool IsAutomatic
);
