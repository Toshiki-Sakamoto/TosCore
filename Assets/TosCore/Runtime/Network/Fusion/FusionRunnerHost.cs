using System.Threading;
using Cysharp.Threading.Tasks;
using Fusion;
using VContainer;

namespace TosCore.Network.Fusion
{
    /// <summary>
    /// Fusion の <see cref="NetworkRunner"/> のライフサイクル（生成・参照・破棄）を管理する。
    /// </summary>
    /// <remarks>
    /// Runner Prefab は DI で注入する。
    /// </remarks>
    public class FusionRunnerHost : IFusionRunnerHost
    {
        private readonly NetworkRunner _runnerPrefab;

        private NetworkRunner _runner;

        [Inject]
        public FusionRunnerHost(NetworkRunner runnerPrefab)
        {
            _runnerPrefab = runnerPrefab;
        }

        /// <summary>生成済みの Runner。未生成なら null。</summary>
        public NetworkRunner Runner => _runner;

        /// <summary>Runner を生成（既に在ればそれを返す）。入力提供を有効にする。</summary>
        public NetworkRunner CreateRunner()
        {
            if (_runner) return _runner;

            _runner = UnityEngine.Object.Instantiate(_runnerPrefab);
            _runner.name = $"{_runnerPrefab.name}(Runtime)";
            _runner.ProvideInput = true;

            return _runner;
        }

        /// <summary>Runner を安全にシャットダウンして破棄する</summary>
        public async UniTask ShutdownRunnerAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            if (_runner == null) return;

            var runner = _runner;
            _runner = null;

            if (runner.IsRunning)
            {
                await runner.Shutdown();
            }

            if (runner != null)
            {
                UnityEngine.Object.Destroy(runner.gameObject);
            }
        }
    }
}
