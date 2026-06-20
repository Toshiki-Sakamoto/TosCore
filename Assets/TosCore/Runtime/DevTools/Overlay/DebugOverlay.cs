using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TosCore.DevTools.Overlay
{
    /// <summary>
    /// 画面上に表示するデバッグOverlay
    /// 外部から IDebugEntry またはラムダ式で表示項目を登録できる
    /// </summary>
    public class DebugOverlay : MonoBehaviour, IDebugOverlay
    {
        /// <summary>
        /// 基準解像度(1080p)でのフォントサイズ。実際の解像度に応じてスケーリングされる
        /// </summary>
        [SerializeField] private int _baseFontSize = 24;

        private const int ReferenceHeight = 1080;
        private readonly List<IDebugEntry> _entries = new();
        private readonly List<IDebugAction> _actions = new();
        private readonly StringBuilder _sb = new();
        private GUIStyle _style;
        private int _cachedScreenHeight;
        private Matrix4x4 _guiMatrix;

        public IDisposable Register(IDebugEntry entry)
        {
            _entries.Add(entry);
            return new EntryHandle(this, entry);
        }

        public IDisposable Register(string label, Func<string> valueProvider, Color? color = null)
        {
            var entry = new LambdaDebugEntry(label, valueProvider, color ?? Color.white);
            return Register(entry);
        }

        public IDisposable RegisterAction(IDebugAction action)
        {
            _actions.Add(action);
            return new ActionHandle(this, action);
        }

        public IDisposable RegisterAction(string label, Action onClick)
        {
            var action = new LambdaDebugAction(label, onClick);
            return RegisterAction(action);
        }

        private void Unregister(IDebugEntry entry)
        {
            _entries.Remove(entry);
        }

        private void UnregisterAction(IDebugAction action)
        {
            _actions.Remove(action);
        }

        private GUIStyle _buttonStyle;

        private void Awake()
        {
            _style = new GUIStyle
            {
                richText = true,
            };
            UpdateScale();
        }

        private void OnGUI()
        {
            if (_cachedScreenHeight != Screen.height)
                UpdateScale();

            var prevMatrix = GUI.matrix;
            GUI.matrix = _guiMatrix;

            _sb.Clear();

            foreach (var entry in _entries)
            {
                entry.Update();
                if (!entry.IsVisible) continue;

                var hex = ColorUtility.ToHtmlStringRGB(entry.Color);
                var text = string.IsNullOrEmpty(entry.Value) ? entry.Label : $"{entry.Label}: {entry.Value}";
                _sb.AppendLine($"<color=#{hex}>{text}</color>");
            }

            // 基準解像度(1080p)ベースの座標。スケーリングで実解像度に合わせる
            GUI.Label(new Rect(10, 10, 600, 900), _sb.ToString(), _style);

            // デバッグボタン描画
            if (_actions.Count > 0)
            {
                _buttonStyle ??= new GUIStyle(GUI.skin.button)
                {
                    fontSize = _baseFontSize,
                };

                var buttonY = 10f;
                const float buttonWidth = 250f;
                const float buttonHeight = 40f;
                // ボタンは右上に配置（基準解像度ベース）
                var buttonX = (float)ReferenceHeight * Screen.width / Screen.height - buttonWidth - 10f;

                foreach (var action in _actions)
                {
                    if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), action.Label,
                            _buttonStyle))
                    {
                        action.OnClick();
                    }
                    buttonY += buttonHeight + 5f;
                }
            }

            GUI.matrix = prevMatrix;
        }

        private void UpdateScale()
        {
            _cachedScreenHeight = Screen.height;
            var scale = (float)Screen.height / ReferenceHeight;
            _guiMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(scale, scale, 1f));
            _style.fontSize = _baseFontSize;
            if (_buttonStyle != null)
                _buttonStyle.fontSize = _baseFontSize;
        }

        private class EntryHandle : IDisposable
        {
            private DebugOverlay _overlay;
            private IDebugEntry _entry;

            public EntryHandle(DebugOverlay overlay, IDebugEntry entry)
            {
                _overlay = overlay;
                _entry = entry;
            }

            public void Dispose()
            {
                _overlay?.Unregister(_entry);
                _overlay = null;
                _entry = null;
            }
        }

        private class ActionHandle : IDisposable
        {
            private DebugOverlay _overlay;
            private IDebugAction _action;

            public ActionHandle(DebugOverlay overlay, IDebugAction action)
            {
                _overlay = overlay;
                _action = action;
            }

            public void Dispose()
            {
                _overlay?.UnregisterAction(_action);
                _overlay = null;
                _action = null;
            }
        }
    }
}
