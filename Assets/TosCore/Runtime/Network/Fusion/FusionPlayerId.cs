using Fusion;

namespace TosCore.Network.Fusion
{
    /// <summary>
    /// Fusion の <see cref="PlayerRef"/> と トランスポート非依存の <see cref="NetworkPlayerId"/> の相互変換。
    /// </summary>
    /// <remarks>
    /// <see cref="NetworkPlayerId"/> の生値は <see cref="PlayerRef.RawEncoded"/> をそのまま保持するため、
    /// 0 同士（None）も含めて往復で一致する。
    /// </remarks>
    public static class FusionPlayerId
    {
        /// <summary><see cref="PlayerRef"/> を <see cref="NetworkPlayerId"/> へ変換する。</summary>
        public static NetworkPlayerId ToNetworkPlayerId(this PlayerRef player) => new NetworkPlayerId(player.RawEncoded);

        /// <summary><see cref="NetworkPlayerId"/> を <see cref="PlayerRef"/> へ変換する。</summary>
        public static PlayerRef ToPlayerRef(this NetworkPlayerId id) => PlayerRef.FromEncoded(id.Raw);
    }
}
