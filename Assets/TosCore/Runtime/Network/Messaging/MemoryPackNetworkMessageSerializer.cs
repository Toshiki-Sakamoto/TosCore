using MemoryPack;

namespace TosCore.Network.Messaging
{
    /// <summary>
    /// MemoryPack を利用したネットワークメッセージシリアライザ。
    /// </summary>
    /// <remarks>
    /// 対象メッセージ型は [MemoryPackable] を付与した struct であること。
    /// </remarks>
    public sealed class MemoryPackNetworkMessageSerializer<TMessage> : INetworkMessageSerializer<TMessage>
        where TMessage : struct
    {
        public byte[] Serialize(in TMessage message)
        {
            return MemoryPackSerializer.Serialize(message);
        }

        public TMessage Deserialize(byte[] payload)
        {
            return MemoryPackSerializer.Deserialize<TMessage>(payload);
        }
    }
}
