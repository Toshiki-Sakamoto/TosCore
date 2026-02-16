namespace TosCore.Bootstrap
{
    /// <summary>
    /// 正規ルート(Bootstrap)経由で起動されたかどうかを管理する
    /// </summary>
    public interface IBootstrapGuard
    {
        /// <summary>
        /// 正規ルートで起動済みかどうか
        /// </summary>
        bool IsBootstrapped { get; }

        /// <summary>
        /// 正規ルートで起動済みとしてマークする
        /// </summary>
        void MarkAsBootstrapped();
    }
}
