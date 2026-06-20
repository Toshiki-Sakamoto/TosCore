namespace TosCore.Network.Messaging
{
    /// <summary>
    /// ネットワークメッセージ（ペイロード）のシリアライズを担当する。
    /// </summary>
    /// <remarks>
    /// 標準実装は MemoryPack ベースの <see cref="MemoryPackNetworkMessageSerializer{TMessage}"/>。
    /// </remarks>
    public interface INetworkMessageSerializer<TMessage>
        where TMessage : struct
    {
        /// <summary>
        /// メッセージを送信用のバイト配列へ変換する。
        /// </summary>
        byte[] Serialize(in TMessage message);

        /// <summary>
        /// 受信したバイト配列からメッセージを復元する。
        /// </summary>
        TMessage Deserialize(byte[] payload);
    }
}
