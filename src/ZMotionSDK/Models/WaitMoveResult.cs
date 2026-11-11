namespace ZMotionSDK.Models;

/// <summary>
/// 等待运动完成的结果
/// </summary>
public struct WaitMoveResult
{
    /// <summary>
    /// 轴号
    /// </summary>
    public int Axis { get; set; }

    /// <summary>
    /// 是否成功完成运动
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// 经过的时间（毫秒）
    /// </summary>
    public double ElapsedTime { get; set; }

    /// <summary>Reason a wait failed; cancellation does not stop hardware motion.</summary>
    public string? FailureReason { get; set; }

    /// <summary>Last observed historical stop code, if available.</summary>
    public int? StopReasonCode { get; set; }

    /// <summary>Last measured position, if requested.</summary>
    public float? FinalPosition { get; set; }

    public override string ToString()
    {
        return $"Axis: {Axis}, IsSuccess: {IsSuccess}, ElapsedTime: {ElapsedTime}ms";
    }
}