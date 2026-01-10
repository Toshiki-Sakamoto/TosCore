using System;
using System.Collections.Generic;

namespace TosCore.TapBlocker
{
    /// <summary>
    /// 1レイヤー分のブロック状態と、そのパネル有効/無効を管理するクラス。
    /// </summary>
    internal sealed class TapBlockLayerState : IDisposable
    {
        private readonly TapBlockerPanel _panel;
        private readonly HashSet<int> _requestIds = new();

        public TapBlockLayerState(TapBlockLayer layer, TapBlockerPanel panel)
        {
            Layer = layer;
            _panel = panel;
        }

        public TapBlockLayer Layer { get; }

        public bool HasRequests => _requestIds.Count > 0;

        public void AddRequest(int id)
        {
            if (!_requestIds.Add(id)) return;
            if (_requestIds.Count == 1)
            {
                _panel.SetActive(true);
            }
        }

        public void RemoveRequest(int id)
        {
            if (!_requestIds.Remove(id)) return;
            if (_requestIds.Count == 0)
            {
                _panel.SetActive(false);
            }
        }

        public void Dispose()
        {
            _requestIds.Clear();
            _panel.Dispose();
        }
    }

}