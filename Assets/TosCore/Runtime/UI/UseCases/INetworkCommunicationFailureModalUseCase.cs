using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace TosCore.UI
{
    public interface INetworkCommunicationFailureModalUseCase
    {
        UniTask<ModalResult<NetworkCommunicationFailureAction>> ShowAsync(CancellationToken ct = default);

        void Show(Action<ModalResult<NetworkCommunicationFailureAction>> onCompleted, CancellationToken ct = default);
    }
}
