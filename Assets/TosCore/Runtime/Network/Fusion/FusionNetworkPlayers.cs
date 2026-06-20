namespace TosCore.Network.Fusion
{
    /// <summary>
    /// Fusion の Runner から <see cref="INetworkPlayers"/> を解決する
    /// </summary>
    public sealed class FusionNetworkPlayers : INetworkPlayers
    {
        private readonly IFusionRunnerHost _runnerHost;

        public FusionNetworkPlayers(IFusionRunnerHost runnerHost)
        {
            _runnerHost = runnerHost;
        }

        public bool IsLocalSpawned
        {
            get
            {
                var runner = _runnerHost.Runner;
                if (runner == null || !runner.IsRunning) return false;

                return runner.GetPlayerObject(runner.LocalPlayer) != null;
            }
        }

        public bool AllSpawned
        {
            get
            {
                var runner = _runnerHost.Runner;
                if (runner == null || !runner.IsRunning) return false;

                var any = false;
                foreach (var player in runner.ActivePlayers)
                {
                    any = true;
                    if (runner.GetPlayerObject(player) == null) return false;
                }

                return any;
            }
        }
    }
}
