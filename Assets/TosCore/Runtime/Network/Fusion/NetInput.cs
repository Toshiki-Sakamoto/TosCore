using Fusion;
using UnityEngine;

namespace TosCore.Network.Fusion
{
    /// <summary>
    /// 1ティック分のネットワーク入力。Fusion の <see cref="INetworkInput"/> として
    /// <c>NetworkRunner.OnInput</c> で詰め、<c>FixedUpdateNetwork()</c> で取り出す。
    /// </summary>
    /// <remarks>
    /// 離散ボタンは <see cref="NetworkButtons"/>（ビットマスク）にまとめ、
    /// ビット位置は <see cref="NetButton"/> で表現する。
    /// </remarks>
    public struct NetInput : INetworkInput
    {
        /// <summary>移動入力（WASD / 左スティック）。各軸 -1..1。</summary>
        public Vector2 Move;

        /// <summary>視点デルタ（マウス移動量 / 右スティック）。</summary>
        public Vector2 Look;

        /// <summary>離散ボタンの押下状態（ビットマスク）。<see cref="NetButton"/> で参照する。</summary>
        public NetworkButtons Buttons;
    }

    /// <summary>
    /// <see cref="NetInput.Buttons"/> のビット位置。
    /// <see cref="NetworkButtons.Set{T}(T,bool)"/> / <see cref="NetworkButtons.WasPressed{T}(NetworkButtons,T)"/> 等で使う。
    /// </summary>
    public enum NetButton
    {
        Jump,
        Sprint,
        Crouch,
        Interact,
    }
}
