using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;

namespace TosCore.UI
{
    /// <summary>
    /// ネットワーク通信失敗モーダルの表示を仲介する
    /// </summary>
    public sealed class NetworkCommunicationFailureModalUseCase : INetworkCommunicationFailureModalUseCase
    {
        private readonly IModalService _modalService;
        private readonly IModalBuilder<NetworkCommunicationFailureAction> _builder;

        [Inject]
        public NetworkCommunicationFailureModalUseCase(
            IModalService modalService,
            IModalBuilder<NetworkCommunicationFailureAction> builder)
        {
            _modalService = modalService;
            _builder = builder;
        }

        public UniTask<ModalResult<NetworkCommunicationFailureAction>> ShowAsync(CancellationToken ct = default)
        {
            return _modalService.TryShowAsync(_builder, ct);
        }

        public void Show(Action<ModalResult<NetworkCommunicationFailureAction>> onCompleted, CancellationToken ct = default)
        {
            _modalService.Show(_builder, onCompleted, ct);
        }
    }
}
