namespace TosCore.Bootstrap
{
    /// <summary>
    /// 「いま直起動補完モードで動いている」ことを表すデバッグ表示用フラグ。
    /// </summary>
    public interface IDirectBootIndicator
    {
        /// <summary>直起動補完モードを有効化する。</summary>
        void Enable();

        /// <summary>直起動補完モードが有効か。</summary>
        bool IsEnabled { get; }
    }
}
