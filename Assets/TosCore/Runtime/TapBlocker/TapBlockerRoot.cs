using UnityEngine;

namespace TosCore.TapBlocker
{
    /// <summary>
    /// タップブロッカー用のCanvasを生成する親Transformを提供するコンポーネント。
    /// </summary>
    public class TapBlockerRoot : MonoBehaviour
    {
        [SerializeField] private RectTransform _overlayRoot;
        [SerializeField] private Vector2 _referenceResolution = new(1920, 1080);
        [SerializeField, Range(0f, 1f)] private float _matchWidthOrHeight = 0.5f;

        public RectTransform OverlayRoot
        {
            get
            {
                if (!_overlayRoot)
                {
                    _overlayRoot = CreateRoot();
                }

                return _overlayRoot;
            }
        }

        public Vector2 ReferenceResolution => _referenceResolution;

        public float MatchWidthOrHeight => _matchWidthOrHeight;

        private RectTransform CreateRoot()
        {
            var go = new GameObject("TapBlockerRoot", typeof(RectTransform));
            var rect = go.GetComponent<RectTransform>();
            rect.SetParent(transform, false);
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.localScale = Vector3.one;
            rect.SetAsLastSibling();
            return rect;
        }
    }
}