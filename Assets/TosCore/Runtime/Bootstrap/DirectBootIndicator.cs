namespace TosCore.Bootstrap
{
    /// <summary>
    /// <see cref="IDirectBootIndicator"/> の既定実装。状態を保持するだけ。
    /// </summary>
    public sealed class DirectBootIndicator : IDirectBootIndicator
    {
        public bool IsEnabled { get; private set; }

        public void Enable()
        {
            IsEnabled = true;
        }
    }
}
