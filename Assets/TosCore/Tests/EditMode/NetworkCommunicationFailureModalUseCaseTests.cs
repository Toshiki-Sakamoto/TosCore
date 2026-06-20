using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using TosCore.UI;
using UnityEngine;

namespace TosCore.Tests.EditMode
{
    public class NetworkCommunicationFailureModalUseCaseTests
    {
        [Test]
        public void ShowAsyncはModalServiceのTryShowAsyncを呼ぶ()
        {
            var modalService = new FakeModalService
            {
                ResultToReturn = ModalResult<NetworkCommunicationFailureAction>.Success(NetworkCommunicationFailureAction.Retry)
            };
            var builder = new EmptyBuilder();
            var useCase = new NetworkCommunicationFailureModalUseCase(modalService, builder);

            var result = useCase.ShowAsync().GetAwaiter().GetResult();

            Assert.That(modalService.TryShowCallCount, Is.EqualTo(1));
            Assert.That(modalService.LastBuilder, Is.SameAs(builder));
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.EqualTo(NetworkCommunicationFailureAction.Retry));
        }

        [Test]
        public void ShowはModalServiceのShowを呼ぶ()
        {
            var modalService = new FakeModalService
            {
                ResultToReturn = ModalResult<NetworkCommunicationFailureAction>.Success(NetworkCommunicationFailureAction.Close)
            };
            var builder = new EmptyBuilder();
            var useCase = new NetworkCommunicationFailureModalUseCase(modalService, builder);
            ModalResult<NetworkCommunicationFailureAction> received = default;
            var invoked = false;

            useCase.Show(result =>
            {
                received = result;
                invoked = true;
            });

            Assert.That(modalService.ShowCallCount, Is.EqualTo(1));
            Assert.That(modalService.LastBuilder, Is.SameAs(builder));
            Assert.That(invoked, Is.True);
            Assert.That(received.IsSuccess, Is.True);
            Assert.That(received.Value, Is.EqualTo(NetworkCommunicationFailureAction.Close));
        }

        private sealed class EmptyBuilder : IModalBuilder<NetworkCommunicationFailureAction>
        {
            public ModalBuildResult<NetworkCommunicationFailureAction> Build(Transform root)
            {
                throw new NotSupportedException();
            }
        }

        private sealed class FakeModalService : IModalService
        {
            public int ActiveCount => 0;
            public event Action<int> Opened;
            public event Action<int> Closed;

            public int TryShowCallCount { get; private set; }
            public int ShowCallCount { get; private set; }
            public IModalBuilder<NetworkCommunicationFailureAction> LastBuilder { get; private set; }
            public ModalResult<NetworkCommunicationFailureAction> ResultToReturn { get; set; }

            public UniTask<TResult> ShowAsync<TResult>(IModalBuilder<TResult> builder, CancellationToken ct = default)
            {
                throw new NotSupportedException();
            }

            public UniTask<ModalResult<TResult>> TryShowAsync<TResult>(IModalBuilder<TResult> builder, CancellationToken ct = default)
            {
                TryShowCallCount++;
                LastBuilder = builder as IModalBuilder<NetworkCommunicationFailureAction>;
                return UniTask.FromResult((ModalResult<TResult>)(object)ResultToReturn);
            }

            public void Show<TResult>(IModalBuilder<TResult> builder, Action<ModalResult<TResult>> onCompleted, CancellationToken ct = default)
            {
                ShowCallCount++;
                LastBuilder = builder as IModalBuilder<NetworkCommunicationFailureAction>;
                onCompleted?.Invoke((ModalResult<TResult>)(object)ResultToReturn);
            }

            public void CloseTop()
            {
            }

            public void CloseAll()
            {
            }
        }
    }
}
