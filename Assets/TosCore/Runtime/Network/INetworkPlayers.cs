namespace TosCore.Network
{
    /// <summary>
    /// ネットワークセッション上のプレイヤーの spawn 状況を引く transport read-model
    /// </summary>
    public interface INetworkPlayers
    {
        /// <summary>ローカルプレイヤーの NetworkObject が spawn 済みか</summary>
        bool IsLocalSpawned { get; }

        /// <summary>アクティブプレイヤーが1人以上いて、その全員が spawn 済みか</summary>
        bool AllSpawned { get; }
    }
}
