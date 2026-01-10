using System;
using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TosCore.Scene
{
    /// <summary>
    /// シーン遷移イベントを受け取り、ライフサイクル終了とシーンロードを仲介するリスナー。
    /// </summary>
    public sealed class SceneTransitionRequestListener : IInitializable, IDisposable
    {
        private readonly ISceneLifecycleRunner _sceneLifecycleRunner;
        private readonly ISceneTransitionService _sceneTransitionService;
        private readonly ISubscriber<SceneTransitionRequestEvent> _subscriber;

        private IDisposable? _subscription;
        private bool _isProcessing;

        [Inject]
        public SceneTransitionRequestListener(
            ISceneLifecycleRunner sceneLifecycleRunner,
            ISceneTransitionService sceneTransitionService,
            ISubscriber<SceneTransitionRequestEvent> subscriber)
        {
            _sceneLifecycleRunner = sceneLifecycleRunner;
            _sceneTransitionService = sceneTransitionService;
            _subscriber = subscriber;
        }

        public void Initialize()
        {
            _subscription = _subscriber.Subscribe(OnSceneTransitionRequest);
        }

        private void OnSceneTransitionRequest(SceneTransitionRequestEvent request)
        {
            if (_isProcessing) return;

            HandleAsync(request).Forget();
        }

        private async UniTaskVoid HandleAsync(SceneTransitionRequestEvent request)
        {
            _isProcessing = true;

            try
            {
                await _sceneLifecycleRunner.ShutdownAsync();
                await _sceneTransitionService.LoadSceneAsync(request.SceneName);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
            finally
            {
                _isProcessing = false;
            }
        }

        public void Dispose()
        {
            _subscription?.Dispose();
            _subscription = null;
        }
    }
}
