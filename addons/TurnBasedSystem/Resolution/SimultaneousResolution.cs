using System.Collections.Generic;
using System.Threading.Tasks;

namespace TurnBasedSystem;

/// <summary>
/// 同时结算 — 所有方规划完毕后同时结算
/// </summary>
public class SimultaneousResolution : IResolutionPolicy
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
        // 同时执行所有行动
        var tasks = new List<Task<ActionResult>>();
        foreach (var action in _pending)
            tasks.Add(action.Execute());
        await Task.WhenAll(tasks);
        _pending.Clear();
    }
}
