using System.Collections.Generic;
using System.Threading.Tasks;

namespace TurnBasedSystem;

/// <summary>
/// 批量结算 — 收集一方所有行动后批量执行
/// </summary>
public class BatchResolution : IResolutionPolicy
{
    private readonly List<ITurnAction> _pending = new();

    public bool HasPending => _pending.Count > 0;

    public Task<ActionResult> Submit(ITurnAction action, TurnContext ctx)
    {
        _pending.Add(action);
        return Task.FromResult(ActionResult.Success);
    }

    public async Task ResolveAll(TurnContext ctx)
    {
        foreach (var action in _pending)
            await action.Execute();
        _pending.Clear();
    }
}
