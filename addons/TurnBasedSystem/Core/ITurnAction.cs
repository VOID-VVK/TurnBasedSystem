using System.Threading.Tasks;

namespace TurnBasedSystem;

/// <summary>
/// 行动接口
/// </summary>
public interface ITurnAction
{
    string ActionName { get; }
    int Cost { get; }
    ITurnUnit Actor { get; }
    Task<ActionResult> Execute();
}
