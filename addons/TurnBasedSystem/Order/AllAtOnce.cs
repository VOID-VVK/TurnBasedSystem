namespace TurnBasedSystem;

/// <summary>
/// 同时行动制 — 文明多人/Into the Breach
/// </summary>
public class AllAtOnce : ITurnOrderResolver
{
    private bool _done;

    public TurnStep? GetNext(TurnContext ctx)
    {
        if (_done) return null;
        _done = true;
        return new TurnStep { IsSimultaneous = true };
    }

    public void OnRoundStart(TurnContext ctx)
    {
        _done = false;
    }

    public void OnActionResolved(TurnContext ctx, ActionResult result) { }

    public void Reset()
    {
        _done = false;
    }
}
