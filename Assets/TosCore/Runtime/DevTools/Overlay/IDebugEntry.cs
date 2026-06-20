using UnityEngine;

namespace TosCore.DevTools.Overlay
{
    /// <summary>
    /// デバッグOverlayに表示するエントリのインターフェース
    /// ラムダでは対応しきれない動的な色変更・表示切替・複雑な更新ロジックに使用する
    /// </summary>
    public interface IDebugEntry
    {
        string Label { get; }
        string Value { get; }
        Color Color { get; }
        bool IsVisible { get; }

        /// <summary>
        /// 毎フレーム呼ばれる更新処理
        /// </summary>
        void Update();
    }
}
