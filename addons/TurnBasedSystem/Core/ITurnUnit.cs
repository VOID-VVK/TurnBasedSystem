namespace TurnBasedSystem;

/// <summary>
/// 参与回合的单位接口
/// </summary>
public interface ITurnUnit
{
    string UnitId { get; }
    string FactionId { get; }
    int Initiative { get; }
    float ATBGauge { get; set; }
    bool IsActive { get; }
    void OnTurnStart();
    void OnTurnEnd();
}
