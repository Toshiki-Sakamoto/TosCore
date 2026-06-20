using UnityEngine.InputSystem;

namespace TosCore.Input
{
    /// <summary>
    /// 入力コンテキスト。アクティブな間だけ有効化される Action Map を1つ提供する。
    /// 基盤は具体的なアクション名（Move / Cancel 等）を一切知らず、Map を渡すだけの薄い窓口。
    /// </summary>
    public interface IInputContext
    {
        /// <summary>コンテキスト識別子（ログ・デバッグ用）。</summary>
        string Id { get; }

        /// <summary>このコンテキストが提供する Action Map。スタックの top の間だけ Enable される。</summary>
        InputActionMap ActionMap { get; }
    }
}
