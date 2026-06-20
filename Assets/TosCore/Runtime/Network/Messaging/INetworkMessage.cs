namespace TosCore.Network.Messaging
{
    /// <summary>
    /// ネットワーク送受信対象メッセージが実装する基底インターフェース。
    /// </summary>
    /// <remarks>
    /// 送信者識別はトランスポート非依存の <see cref="NetworkPlayerId"/> で表す。
    /// </remarks>
    public interface INetworkMessage
    {
        /// <summary>このメッセージを送ったプレイヤー。</summary>
        NetworkPlayerId Sender { get; set; }
    }
}
