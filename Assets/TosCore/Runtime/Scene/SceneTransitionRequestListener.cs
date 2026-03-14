using System;
using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TosCore.Scene
{
    /// <summary>
    /// シーン遷移イベントを受け取り、ユースケースへ委譲する入力アダプタ。
    /// </summary>
    public sealed class SceneTransitionRequestListener : IInitializable, IDisposable
    {
        private readonly ISceneTransitionUseCase _useCase;
        private readonly ISubscriber<SceneTransitionRequestEvent> _subscriber;

        private IDisposable? _subscription;

        [Inject]
        public SceneTransitionRequestListener(
            ISceneTransitionUseCase useCase,
            ISubscriber<SceneTransitionRequestEvent> subscriber)
        {
            _useCase = useCase;
            _subscriber = subscriber;
        }

        public void Initialize()
        {
            _subscription = _subscriber.Subscribe(OnSceneTransitionRequest);
        }

        private void OnSceneTransitionRequest(SceneTransitionRequestEvent request)
        {
            HandleAsync(request).Forget();
        }

        private async UniTaskVoid HandleAsync(SceneTransitionRequestEvent request)
        {
            try
            {
                await _useCase.ExecuteAsync(request.SceneName);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        public void Dispose()
        {
            _subscription?.Dispose();
            _subscription = null;
        }
    }
}
