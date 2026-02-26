namespace TurnBasedSystem;

/// <summary>
/// 四维组合配置 — 定义一个回合制系统的完整行为
/// </summary>
public class TurnSystemConfig
{
    public ITurnOrderResolver OrderResolver { get; }
    public IActionBudget ActionBudget { get; }
    public IResolutionPolicy ResolutionPolicy { get; }
    public ITurnPhaseSequence PhaseSequence { get; }

    public TurnSystemConfig(
        ITurnOrderResolver orderResolver,
        IActionBudget actionBudget,
        IResolutionPolicy resolutionPolicy,
        ITurnPhaseSequence phaseSequence)
    {
        OrderResolver = orderResolver;
        ActionBudget = actionBudget;
        ResolutionPolicy = resolutionPolicy;
        PhaseSequence = phaseSequence;
    }
}
