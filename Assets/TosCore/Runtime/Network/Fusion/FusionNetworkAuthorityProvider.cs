using Fusion;
using VContainer;

namespace TosCore.Network.Fusion
{
    /// <summary>
    /// Fusion の <see cref="NetworkRunner"/> を参照してローカル権限の有無を解決する。
    /// </summary>
    public class FusionNetworkAuthorityProvider : INetworkAuthorityProvider
    {
        private readonly IFusionRunnerHost _runnerHost;

        [Inject]
        public FusionNetworkAuthorityProvider(IFusionRunnerHost runnerHost)
        {
            _runnerHost = runnerHost;
        }

        public bool IsAuthority
        {
            get
            {
                var runner = _runnerHost.Runner;
                if (runner == null || !runner.IsRunning) return false;

                // Single モードは唯一のプレイヤーが権限保持者
                if (runner.GameMode == GameMode.Single) return true;

                // Shared モードは MasterClient が権限保持者
                return runner.IsSharedModeMasterClient;
            }
        }
    }
}
