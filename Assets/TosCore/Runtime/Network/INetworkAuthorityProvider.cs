namespace TosCore.Network
{
    /// <summary>
    /// ネットワーク全体（セッション）におけるローカルクライアントの権限保持状況の抽象。
    /// </summary>
    /// <remarks>
    /// 「このクライアントがセッション全体の Master 役か」を表す。
    /// </remarks>
    public interface INetworkAuthorityProvider
    {
        /// <summary>このクライアントがネットワーク全体の権限保持者か。</summary>
        bool IsAuthority { get; }
    }
}
