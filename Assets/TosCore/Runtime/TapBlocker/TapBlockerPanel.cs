using System;
using UnityEngine;
using UnityEngine.UI;

namespace TosCore.TapBlocker
{
    /// <summary>
    /// タップブロッカーGameObjectの生成、管理を行う
    /// </summary>
    internal sealed class TapBlockerPanel : IDisposable
    {
        private readonly GameObject _root;
        private readonly Canvas _canvas;

        public TapBlockerPanel(RectTransform parent, Camera camera, Vector2 referenceResolution, float matchWidthOrHeight, int sortingOrder)
        {
            _root = new GameObject($"TapBlockerLayer_{sortingOrder}", typeof(RectTransform));
            var rectTransform = _root.GetComponent<RectTransform>();
            rectTransform.SetParent(parent, false);
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);

            var uiLayer = LayerMask.NameToLayer("UI");
            _root.layer = uiLayer >= 0 ? uiLayer : 5;

            _canvas = _root.AddComponent<Canvas>();
            var hasCamera = camera != null;
            _canvas.renderMode = hasCamera ? RenderMode.ScreenSpaceCamera : RenderMode.ScreenSpaceOverlay;
            _canvas.overrideSorting = true;
            _canvas.sortingOrder = sortingOrder;
            if (hasCamera)
            {
                _canvas.worldCamera = camera;
            }

            var scaler = _root.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = referenceResolution;
            scaler.matchWidthOrHeight = Mathf.Clamp01(matchWidthOrHeight);

            _root.AddComponent<GraphicRaycaster>();

            var image = _root.AddComponent<Image>();
            image.color = new Color(0, 0, 0, 0);
            image.raycastTarget = true;

            _root.SetActive(false);
        }

        public void SetActive(bool active)
        {
            if (_root) _root.SetActive(active);
        }

        public void Dispose()
        {
            if (_root)
            {
                UnityEngine.Object.Destroy(_root);
            }
        }
    }
}