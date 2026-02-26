namespace TurnBasedSystem;

/// <summary>
/// 8 种回合制预设模板
/// </summary>
public static class TurnTemplates
{
    /// <summary>象棋/围棋 — 双方严格交替</summary>
    public static TurnSystemConfig Classic() => new(
        new AlternatingOrder(),
        new SingleAction(),
        new ImmediateResolution(),
        new SinglePhase());

    /// <summary>火焰纹章/XCOM/大泽乡 — 阵营全体行动</summary>
    public static TurnSystemConfig Faction(int factionCount = 2) => new(
        new FactionRoundRobin(factionCount),
        new SingleAction(),
        new BatchResolution(),
        new SinglePhase());

    /// <summary>博德之门3/FFX — 速度先攻制</summary>
    public static TurnSystemConfig Initiative(bool recalcAfterAction = false) => new(
        new InitiativeQueue(recalcAfterAction),
        new SingleAction(),
        new ImmediateResolution(),
        new SinglePhase());

    /// <summary>FF ATB — 半即时制</summary>
    public static TurnSystemConfig ATB(float gaugeSpeed = 1.0f) => new(
        new ATBGauge(gaugeSpeed),
        new GaugeBased(),
        new ImmediateResolution(),
        new SinglePhase());

    /// <summary>女神异闻录 — 弱点连锁制</summary>
    public static TurnSystemConfig PressTurn(int iconsPerTurn = 4) => new(
        new ConditionalOrder(),
        new PressTurnIcons(iconsPerTurn),
        new ImmediateResolution(),
        new PressTurnPhases());

    /// <summary>文明多人/Into the Breach — 同时行动制</summary>
    public static TurnSystemConfig Simultaneous() => new(
        new AllAtOnce(),
        new SingleAction(),
        new SimultaneousResolution(),
        new PlanResolve());

    /// <summary>杀戮尖塔/神界原罪2 — 资源驱动制</summary>
    public static TurnSystemConfig Resource(int defaultAP = 3) => new(
        new FactionRoundRobin(2),
        new ActionPoints(defaultAP),
        new ImmediateResolution(),
        new CardPhases());

    /// <summary>Nethack/不思议迷宫 — Roguelike 滴答制</summary>
    public static TurnSystemConfig Tick() => new(
        new TickOrder(),
        new SingleAction(),
        new ImmediateResolution(),
        new SinglePhase());
}
