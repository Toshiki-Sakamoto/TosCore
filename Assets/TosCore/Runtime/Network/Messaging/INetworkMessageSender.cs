namespace TosCore.Network.Messaging
{
    /// <summary>
    /// ネットワークメッセージ送信を行う。
    /// </summary>
    public interface INetworkMessageSender
    {
        /// <summary>
        /// 指定メッセージをネットワークへ送信する。
        /// </summary>
        void Send<TMessage>(in TMessage message) where TMessage : struct, INetworkMessage;
    }
}
