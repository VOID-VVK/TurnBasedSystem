namespace TurnBasedSystem;

/// <summary>
/// 行动结果枚举
/// </summary>
public enum ActionResult
{
    Success,
    Failed,
    Critical,
    Missed,
    Reflected,
    Blocked,
    InsufficientResources
}
