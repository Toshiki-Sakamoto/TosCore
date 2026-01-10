using System.Collections.Generic;

namespace TosCore.TapBlocker
{
    public interface ITapBlockerService
    {
        /// <summary>
        /// 指定したレイヤーのタップ入力を一時的に遮断する。
        /// </summary>
        /// <param name="layer">遮断したいレイヤー（ソート順情報含む）。</param>
        /// <param name="reason">デバッグ用に残す理由ラベル。</param>
        /// <returns>Dispose 時に自動で解除されるハンドル。</returns>
        TapBlockHandle Acquire(TapBlockLayer layer, string reason = null);

        bool IsBlocked(TapBlockLayer layer);

        IReadOnlyList<TapBlockRequestSnapshot> ActiveRequests { get; }
    }

    /// <summary>
    /// 現在のブロック要求を確認するためのスナップショット。
    /// </summary>
    public readonly struct TapBlockRequestSnapshot
    {
        public TapBlockRequestSnapshot(int id, TapBlockLayer layer, string reason)
        {
            Id = id;
            Layer = layer;
            Reason = reason;
        }

        public int Id { get; }

        public TapBlockLayer Layer { get; }

        public string Reason { get; }
    }

}