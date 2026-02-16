using VContainer.Unity;

namespace TosCore.Bootstrap
{
    /// <summary>
    /// 正規ルート(Bootstrap)のLifetimeScopeでEntryPointとして登録する
    /// IInitializable.Initialize()で BootstrapGuard をマークする
    /// </summary>
    public sealed class BootstrapInitializer : IInitializable
    {
        private readonly IBootstrapGuard _guard;

        public BootstrapInitializer(IBootstrapGuard guard)
        {
            _guard = guard;
        }

        public void Initialize()
        {
            _guard.MarkAsBootstrapped();
        }
    }
}
