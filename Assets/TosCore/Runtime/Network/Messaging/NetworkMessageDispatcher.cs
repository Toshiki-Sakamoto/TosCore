using System;
using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace TosCore.Network.Messaging
{
    /// <summary>
    /// 受信した Envelope を購読し、登録済みの型別 Publisher（MessagePipe）へ中継（ディスパッチ）する。
    /// </summary>
    /// <remarks>
    /// <see cref="ISubscriber{T}"/> 経由で <see cref="NetworkMessageEnvelopeReceivedMessage"/> を購読し、
    /// <see cref="NetworkMessageRegistry.TryPublish"/> で元の型に復元して Publish する。
    /// VContainer の <see cref="IInitializable"/> として登録して使う。
    /// </remarks>
    public sealed class NetworkMessageDispatcher : IInitializable, IDisposable
    {
        private readonly ISubscriber<NetworkMessageEnvelopeReceivedMessage> _networkMessageEnvelopeReceivedSubscriber;
        private readonly NetworkMessageRegistry _networkMessageRegistry;

        private IDisposable _disposable = DisposableBag.Empty;

        [Inject]
        public NetworkMessageDispatcher(
            ISubscriber<NetworkMessageEnvelopeReceivedMessage> networkMessageEnvelopeReceivedSubscriber,
            NetworkMessageRegistry networkMessageRegistry)
        {
            _networkMessageEnvelopeReceivedSubscriber = networkMessageEnvelopeReceivedSubscriber;
            _networkMessageRegistry = networkMessageRegistry;
        }

        public void Initialize()
        {
            var disposableBag = DisposableBag.CreateBuilder();
            _networkMessageEnvelopeReceivedSubscriber.Subscribe(OnReceived).AddTo(disposableBag);
            _disposable = disposableBag.Build();
        }

        public void Dispose()
        {
            _disposable.Dispose();
            _disposable = DisposableBag.Empty;
        }

        private void OnReceived(NetworkMessageEnvelopeReceivedMessage message)
        {
            // 型に変換して再度 Publish する
            _networkMessageRegistry.TryPublish(message.Envelope);
        }
    }
}
