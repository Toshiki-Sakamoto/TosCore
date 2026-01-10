using System;
using System.Collections.Generic;
using TosCore.UI;
using UnityEngine;
using VContainer;

namespace TosCore.TapBlocker
{
    /// <summary>
    /// 透明パネルを生成してタップ入力をレイヤー単位で遮断するサービス
    /// </summary>
    public sealed class TapBlockerService : ITapBlockerService, IDisposable
    {
        private readonly TapBlockerRoot _root;
        private readonly UICameraProvider _cameraProvider;
        private readonly TapBlockRegistry _registry;
        private int _nextRequestId = 1;
        private bool _disposed;

        [Inject]
        public TapBlockerService(
            TapBlockerRoot root, 
            UICameraProvider cameraProvider)
        {
            _root = root;
            _cameraProvider = cameraProvider;
            _registry = new TapBlockRegistry(CreateLayerState);
        }

        public IReadOnlyList<TapBlockRequestSnapshot> ActiveRequests => _registry.Snapshots;

        public TapBlockHandle Acquire(TapBlockLayer layer, string reason = null)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(TapBlockerService));

            var id = _nextRequestId++;
            _registry.Acquire(layer, id, reason ?? string.Empty);

            return new TapBlockHandle(this, id);
        }

        public bool IsBlocked(TapBlockLayer layer) => _registry.IsLayerBlocked(layer);

        internal void Release(int requestId)
        {
            _registry.Release(requestId);
        }

        /// <summary>
        /// ブロック用の一枚を生成
        /// </summary>
        private TapBlockLayerState CreateLayerState(TapBlockLayer layer)
        {
            var panel = new TapBlockerPanel(
                _root.OverlayRoot,
                ResolveCamera(),
                _root.ReferenceResolution,
                _root.MatchWidthOrHeight,
                layer.SortingOrder);
            return new TapBlockLayerState(layer, panel);
        }

        private Camera ResolveCamera() => _cameraProvider && _cameraProvider.Camera
            ? _cameraProvider.Camera
            : Camera.main;

        public void Dispose()
        {
            if (_disposed) return;
        
            _disposed = true;
            _registry.Dispose();
        }
    }
}