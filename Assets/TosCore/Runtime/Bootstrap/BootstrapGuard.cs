namespace TosCore.Bootstrap
{
    /// <summary>
    /// 正規ルート(Bootstrap)経由で起動されたかどうかを管理する
    /// </summary>
    public sealed class BootstrapGuard : IBootstrapGuard
    {
        public bool IsBootstrapped { get; private set; }

        public void MarkAsBootstrapped()
        {
            IsBootstrapped = true;
        }
    }
}
