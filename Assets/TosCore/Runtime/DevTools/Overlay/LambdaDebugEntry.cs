using System;
using UnityEngine;

namespace TosCore.DevTools.Overlay
{
    /// <summary>
    /// ラムダ式による簡易デバッグエントリ
    /// </summary>
    internal class LambdaDebugEntry : IDebugEntry
    {
        private readonly Func<string> _valueProvider;

        public string Label { get; }
        public string Value { get; private set; }
        public Color Color { get; }
        public bool IsVisible => true;

        public LambdaDebugEntry(string label, Func<string> valueProvider, Color color)
        {
            Label = label;
            _valueProvider = valueProvider;
            Color = color;
        }

        public void Update()
        {
            Value = _valueProvider();
        }
    }
}
