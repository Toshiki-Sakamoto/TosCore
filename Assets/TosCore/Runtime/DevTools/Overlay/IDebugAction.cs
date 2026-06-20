namespace TosCore.DevTools.Overlay
{
    /// <summary>
    /// デバッグOverlayに表示するボタンのインターフェース
    /// </summary>
    public interface IDebugAction
    {
        string Label { get; }
        void OnClick();
    }
}
