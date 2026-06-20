using MemoryPack;

namespace TosCore.Network.Messaging
{
    /// <summary>
    /// ネットワーク送受信に利用する共通包み。実際にネットワークへ送られるのはこの構造体。
    /// </summary>
    /// <remarks>
    /// シリアライズには MemoryPack（[MemoryPackable]）を使用する。
    ///
    /// 送信者識別子はトランスポート非依存の <see cref="NetworkPlayerId"/> で表す。
    /// wire 上はエンコード済み int (<see cref="SenderRaw"/>) を直列化対象とし、
    /// <see cref="Sender"/>（[MemoryPackIgnore]）で <see cref="NetworkPlayerId"/> と相互変換する。
    /// SenderRaw 既定値 0 は <see cref="NetworkPlayerId.None"/> に一致する。
    /// </remarks>
    [MemoryPackable]
    public partial struct NetworkMessageEnvelope
    {
        [MemoryPackConstructor]
        public NetworkMessageEnvelope(ushort messageId, int senderRaw, byte[] payload)
        {
            MessageId = messageId;
            SenderRaw = senderRaw;
            Payload = payload;
        }

        public NetworkMessageEnvelope(ushort messageId, NetworkPlayerId sender, byte[] payload)
            : this(messageId, sender.Raw, payload)
        {
        }

        public ushort MessageId { get; set; }

        /// <summary>
        /// 送信者識別子のエンコード済み生値（MemoryPack 直列化対象）。
        /// </summary>
        public int SenderRaw { get; set; }

        public byte[] Payload { get; set; }

        /// <summary>
        /// 送信者を <see cref="NetworkPlayerId"/> として読み書きする便宜プロパティ。
        /// 実体は <see cref="SenderRaw"/>（[MemoryPackIgnore] のため直列化されない）。
        /// </summary>
        [MemoryPackIgnore]
        public NetworkPlayerId Sender
        {
            get => new NetworkPlayerId(SenderRaw);
            set => SenderRaw = value.Raw;
        }
    }
}
