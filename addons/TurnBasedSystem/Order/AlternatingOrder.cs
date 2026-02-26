namespace TurnBasedSystem;

/// <summary>
/// 双方严格交替 — 象棋/围棋
/// </summary>
public class AlternatingOrder : ITurnOrderResolver
{
    private int _currentIndex;
    private bool _acted;

    public TurnStep? GetNext(TurnContext ctx)
    {
        if (_acted) return null;
        _acted = true;

        var factions = ctx.Factions;
        if (factions.Count == 0) return null;

        var faction = factions[_currentIndex % factions.Count];
        var units = faction.GetActiveUnits();
        if (units.Count == 0) return null;

        return new TurnStep { Unit = units[0], Faction = faction };
    }

    public void OnRoundStart(TurnContext ctx)
    {
        _acted = false;
    }

    public void OnActionResolved(TurnContext ctx, ActionResult result)
    {
        _currentIndex++;
    }

    public void Reset()
    {
        _currentIndex = 0;
        _acted = false;
    }
}
