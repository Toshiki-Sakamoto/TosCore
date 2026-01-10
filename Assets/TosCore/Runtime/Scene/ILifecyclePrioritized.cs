namespace TosCore.Scene
{
    /// <summary>
    /// ライフサイクルのプライオリティ
    /// 値が小さいほど早く実行される
    /// </summary>
    public interface ILifecyclePrioritized
    {
        int Priority { get; }
    }
}
