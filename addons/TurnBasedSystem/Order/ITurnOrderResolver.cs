namespace TurnBasedSystem;

/// <summary>
/// 维度1: 行动顺序解析器
/// </summary>
public interface ITurnOrderResolver
{
    TurnStep? GetNext(TurnContext ctx);
    void OnRoundStart(TurnContext ctx);
    void OnActionResolved(TurnContext ctx, ActionResult result);
    void Reset();
}
