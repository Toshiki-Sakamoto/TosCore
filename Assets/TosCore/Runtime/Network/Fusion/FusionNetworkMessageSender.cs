using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using MemoryPack;
using MessagePipe;
using TosCore.Network.Messaging;
using VContainer;

namespace TosCore.Network.Fusion
{
    /// <summary>
    /// アプリ側のメッセージ送信と Fusion の Reliable Data 送受信の橋渡しをする。
    /// 送信（<see cref="INetworkMessageSender"/>）と受信（<see cref="INetworkRunnerCallbacks"/> の
    /// OnReliableDataReceived）の双方を担う。
    /// </summary>
    /// <remarks>
    /// 受信は MessagePipe（<see cref="IPublisher{T}"/>）で <see cref="NetworkMessageEnvelopeReceivedMessage"/> を Publish し、
    /// <see cref="NetworkMessageDispatcher"/> がそれを購読して型別 Publisher へ中継する。
    ///
    /// 利用側は Runner 生成後に runner.AddCallbacks(thisInstance) を呼んでコールバックを登録すること。
    /// </remarks>
    public class FusionNetworkMessageSender : INetworkMessageSender, INetworkRunnerCallbacks
    {
        /// <summary>
        /// Reliable Data の宛先キー。送受信で必ず一致させる固定値（同一値同士でなければ受信側で弾かれる）。
        /// </summary>
        private static readonly ReliableKey NetworkMessageReliableKey =
            ReliableKey.FromInts(unchecked((int)0x4a534d4e), unchecked((int)0x4e54574b), 0, 1);

        private readonly IFusionRunnerHost _fusionRunnerHost;
        private readonly NetworkMessageRegistry _networkMessageRegistry;
        private readonly IPublisher<NetworkMessageEnvelopeReceivedMessage> _networkMessageEnvelopeReceivedPublisher;

        [Inject]
        public FusionNetworkMessageSender(
            IFusionRunnerHost fusionRunnerHost,
            NetworkMessageRegistry networkMessageRegistry,
            IPublisher<NetworkMessageEnvelopeReceivedMessage> networkMessageEnvelopeReceivedPublisher)
        {
            _fusionRunnerHost = fusionRunnerHost;
            _networkMessageRegistry = networkMessageRegistry;
            _networkMessageEnvelopeReceivedPublisher = networkMessageEnvelopeReceivedPublisher;
        }

        public void Send<TMessage>(in TMessage message)
            where TMessage : struct, INetworkMessage
        {
            var runner = _fusionRunnerHost.Runner;

            // Runner 未生成 / 未稼働ならネットワーク越しには送らない。
            if (runner == null || !runner.IsRunning) return;

            var messageWithSender = message;

            // 送信者未設定（None）なら自分（ローカルプレイヤー）を送信者とする。
            if (messageWithSender.Sender.IsNone)
            {
                messageWithSender.Sender = runner.LocalPlayer.ToNetworkPlayerId();
            }

            // メッセージ送信本体（Envelope）を作成。
            var envelope = _networkMessageRegistry.CreateEnvelope(messageWithSender);

            // MemoryPack でシリアライズ。
            var data = MemoryPackSerializer.Serialize(envelope);

            // Shared モードでは各自が他プレイヤー全員へ直接送る。
            SendToOtherPlayers(runner, data);
        }

        /// <summary>
        /// Reliable Data 受信時のコールバック。
        /// </summary>
        void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
        {
            // 自分宛て（このメッセージング層）のキーかどうか。
            if (key != NetworkMessageReliableKey) return;

            // デシリアライズできるか。
            if (!TryDeserializeEnvelope(data, out var envelope)) return;

            _networkMessageEnvelopeReceivedPublisher.Publish(new NetworkMessageEnvelopeReceivedMessage(envelope));
        }

        private static bool TryDeserializeEnvelope(ArraySegment<byte> data, out NetworkMessageEnvelope envelope)
        {
            envelope = default;
            try
            {
                var bytes = CopySegmentToArray(data);
                envelope = MemoryPackSerializer.Deserialize<NetworkMessageEnvelope>(bytes);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 受信データ（セグメント）を独立した byte 配列へコピーする。
        /// </summary>
        private static byte[] CopySegmentToArray(ArraySegment<byte> data)
        {
            if (data.Array == null || data.Count <= 0)
            {
                return Array.Empty<byte>();
            }

            var bytes = new byte[data.Count];
            Buffer.BlockCopy(data.Array, data.Offset, bytes, 0, data.Count);
            return bytes;
        }

        /// <summary>
        /// ローカル以外の全プレイヤーへ data を送信する。
        /// </summary>
        private static void SendToOtherPlayers(NetworkRunner runner, byte[] data)
        {
            foreach (var targetPlayer in runner.ActivePlayers)
            {
                if (targetPlayer == runner.LocalPlayer) continue;

                runner.SendReliableDataToPlayer(targetPlayer, NetworkMessageReliableKey, data);
            }
        }

        void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
        }

        void INetworkRunnerCallbacks.OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        void INetworkRunnerCallbacks.OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
        }

        void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input)
        {
        }

        void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner)
        {
        }

        void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
        }

        void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
        }

        void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        void INetworkRunnerCallbacks.OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
        }

        void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner)
        {
        }

        void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner)
        {
        }
    }
}
