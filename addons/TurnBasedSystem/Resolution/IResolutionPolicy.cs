using System.Threading.Tasks;

namespace TurnBasedSystem;

/// <summary>
/// 维度3: 结算策略
/// </summary>
public interface IResolutionPolicy
{
    Task<ActionResult> Submit(ITurnAction action, TurnContext ctx);
    Task ResolveAll(TurnContext ctx);
    bool HasPending { get; }
}
