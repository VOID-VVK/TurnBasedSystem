namespace TurnBasedSystem;

/// <summary>
/// 阵营全体行动 — 火焰纹章/XCOM/大泽乡
/// </summary>
public class FactionRoundRobin : ITurnOrderResolver
{
    private readonly int _factionCount;
    private int _currentFactionIndex;

    public FactionRoundRobin(int factionCount)
    {
        _factionCount = factionCount;
    }

    public TurnStep? GetNext(TurnContext ctx)
    {
        var factions = ctx.Factions;
        if (_currentFactionIndex >= factions.Count) return null;

        var step = new TurnStep { Faction = factions[_currentFactionIndex] };
        _currentFactionIndex++;
        return step;
    }

    public void OnRoundStart(TurnContext ctx)
    {
        _currentFactionIndex = 0;
    }

    public void OnActionResolved(TurnContext ctx, ActionResult result) { }

    public void Reset()
    {
        _currentFactionIndex = 0;
    }
}
