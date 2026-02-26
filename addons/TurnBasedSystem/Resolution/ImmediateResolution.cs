using System.Threading.Tasks;

namespace TurnBasedSystem;

/// <summary>
/// 立即结算 — 提交即执行
/// </summary>
public class ImmediateResolution : IResolutionPolicy
{
    public bool HasPending => false;

    public async Task<ActionResult> Submit(ITurnAction action, TurnContext ctx)
    {
        return await action.Execute();
    }

    public Task ResolveAll(TurnContext ctx) => Task.CompletedTask;
}
