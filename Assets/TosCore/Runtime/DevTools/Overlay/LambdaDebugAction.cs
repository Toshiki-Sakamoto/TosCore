using System;

namespace TosCore.DevTools.Overlay
{
    /// <summary>
    /// ラムダ式による簡易デバッグボタン
    /// </summary>
    internal class LambdaDebugAction : IDebugAction
    {
        private readonly Action _onClick;

        public string Label { get; }

        public LambdaDebugAction(string label, Action onClick)
        {
            Label = label;
            _onClick = onClick;
        }

        public void OnClick()
        {
            _onClick?.Invoke();
        }
    }
}
