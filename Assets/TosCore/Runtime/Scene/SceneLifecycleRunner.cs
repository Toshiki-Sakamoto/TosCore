using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TosCore.Bootstrap;
using UnityEngine;

namespace TosCore.Scene
{
    public sealed class SceneLifecycleRunner : ISceneLifecycleRunner
    {
        private readonly ISceneInitializer _initializer;
        private readonly ISceneStarter _starter;
        private readonly ISceneEnterer _enterer;
        private readonly ISceneUpdater _updater;
        private readonly ISceneExiter _exiter;
        private readonly IBootstrapGuard _bootstrapGuard;
        private readonly IDirectBootstrapper _directBootstrapper;
        private readonly IEnumerable<ISceneInitializable> _initializables;
        private readonly IEnumerable<ISceneStartable> _startables;
        private readonly IEnumerable<ISceneEnterable> _enterables;
        private readonly IEnumerable<ISceneTickable> _tickables;
        private readonly IEnumerable<ISceneExitable> _exitables;

        private readonly CancellationTokenSource _cts = new();
        private SceneLifecycleState _state = SceneLifecycleState.None;
        private bool _shutdownInvoked;


        public SceneLifecycleState State => _state;

        public SceneLifecycleRunner(
            ISceneInitializer initializer,
            ISceneStarter starter,
            ISceneEnterer enterer,
            ISceneUpdater updater,
            ISceneExiter exiter,
            IBootstrapGuard bootstrapGuard,
            IDirectBootstrapper directBootstrapper,
            IEnumerable<ISceneInitializable> initializables,
            IEnumerable<ISceneStartable> startables,
            IEnumerable<ISceneEnterable> enterables,
            IEnumerable<ISceneTickable> tickables,
            IEnumerable<ISceneExitable> exitables)
        {
            _initializer = initializer;
            _starter = starter;
            _enterer = enterer;
            _updater = updater;
            _exiter = exiter;
            _bootstrapGuard = bootstrapGuard;
            _directBootstrapper = directBootstrapper;
            _initializables = initializables ?? Array.Empty<ISceneInitializable>();
            _startables = startables ?? Array.Empty<ISceneStartable>();
            _enterables = enterables ?? Array.Empty<ISceneEnterable>();
            _tickables = tickables ?? Array.Empty<ISceneTickable>();
            _exitables = exitables ?? Array.Empty<ISceneExitable>();
        }


        public void PostInitialize()
        {
            // 開始時にシーンの初期化をまずは走らせる
            _initializer.Initialize(_initializables);
            _starter.Initialize(_startables);
            _enterer.Initialize(_enterables);
            _updater.Initialize(_tickables);
            _exiter.Initialize(_exitables);

            RunInitializationAsync();
        }

        public void Tick()
        {
            if (_state != SceneLifecycleState.Updating) return;

            _updater.Tick(Time.deltaTime);
        }

        public void Dispose()
        {
            ShutdownAsync().Forget();
        }

        private async UniTask RunInitializationAsync()
        {
            _state = SceneLifecycleState.Initializing;

            try
            {
                // 正規ルート外（Editor直起動等）の場合、補完初期化を実行
                if (!_bootstrapGuard.IsBootstrapped)
                {
                    await _directBootstrapper.BootstrapAsync(_cts.Token);
                    _bootstrapGuard.MarkAsBootstrapped();
                }

                if (_initializer.HasInitializers)
                {
                    await _initializer.RunAsync(_cts.Token);
                }

                if (_enterer.HasEnterables)
                {
                    await _enterer.RunAsync(_cts.Token);
                }

                if (!_shutdownInvoked)
                {
                    _state = SceneLifecycleState.Starting;
                }
            }
            catch (OperationCanceledException)
            {
                // シャットダウン要求を受け取ったため無視
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                await ShutdownAsync();
            }
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            // 次にStart
            _state = SceneLifecycleState.Starting;

            if (_starter.HasStartables)
            {
                await _starter.RunAsync(_cts.Token);
            }

            if (!_shutdownInvoked)
            {
                _state = SceneLifecycleState.Updating;
            }
        }

        public UniTask ShutdownAsync()
        {
            if (_shutdownInvoked)
            {
                return UniTask.CompletedTask;
            }

            _shutdownInvoked = true;
            return ShutdownInternalAsync();
        }

        private async UniTask ShutdownInternalAsync()
        {
            if (_state != SceneLifecycleState.Exiting && _state != SceneLifecycleState.Exited)
            {
                _state = SceneLifecycleState.Exiting;
            }

            if (!_cts.IsCancellationRequested)
            {
                _cts.Cancel();
            }

            try
            {
                if (_exiter.HasExitables)
                {
                    await _exiter.RunAsync(CancellationToken.None);
                }
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
            finally
            {
                _state = SceneLifecycleState.Exited;
                _cts.Dispose();
            }
        }
    }
}
