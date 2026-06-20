using System;

namespace TosCore.Network
{
    /// <summary>
    /// ネットワーク上の送信者を表すトランスポート非依存の識別子。
    /// </summary>
    /// <remarks>
    /// 実体はエンコード済みの生値(int)。Fusion を使う場合は PlayerRef.RawEncoded をそのまま格納し、
    /// Fusion 層(TosCore.Network.Fusion)で PlayerRef と相互変換する。
    /// 既定値(Raw=0)は「未設定 / None」を表す。
    /// </remarks>
    public readonly struct NetworkPlayerId : IEquatable<NetworkPlayerId>
    {
        /// <summary>未設定（None）を表す識別子。</summary>
        public static readonly NetworkPlayerId None = default;

        /// <summary>エンコード済みの生値。</summary>
        public int Raw { get; }

        public NetworkPlayerId(int raw)
        {
            Raw = raw;
        }

        /// <summary>未設定（None）かどうか。</summary>
        public bool IsNone => Raw == 0;

        public bool Equals(NetworkPlayerId other) => Raw == other.Raw;
        public override bool Equals(object obj) => obj is NetworkPlayerId other && Equals(other);
        public override int GetHashCode() => Raw;
        public static bool operator ==(NetworkPlayerId left, NetworkPlayerId right) => left.Equals(right);
        public static bool operator !=(NetworkPlayerId left, NetworkPlayerId right) => !left.Equals(right);
        public override string ToString() => IsNone ? "None" : $"Player({Raw})";
    }
}
