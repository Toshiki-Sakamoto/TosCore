using UnityEngine;

namespace TosCore.UI
{
    public class UICameraRebinder : MonoBehaviour
    {
        [SerializeField] private bool _disableThiscameraAfterRebind = false;

        private void Awake()
        {
            var globalCamera = UICameraProvider.Instance.Camera;
            var thisCamera = GetComponent<Camera>();
            if (globalCamera == null || thisCamera == null) return;
            
            // このカメラが属してるシーンだけ見る
            var scene = gameObject.scene;

            // そのシーンのルートGameObjectを全部取る
            var roots = scene.GetRootGameObjects();

            foreach (var root in roots)
            {
                // 子孫のCanvasを全部集める（非アクティブ含めたいなら true にする）
                var canvases = root.GetComponentsInChildren<Canvas>(includeInactive: true);

                foreach (var canvas in canvases)
                {
                    if (canvas.renderMode != RenderMode.ScreenSpaceCamera &&
                        canvas.renderMode != RenderMode.WorldSpace) continue;
                    
                    if (canvas.worldCamera == thisCamera)
                    {
                        canvas.worldCamera = globalCamera;
                    }
                }
            }

            // 役目が終わったこのローカルUICameraは無効化
            gameObject.SetActive(_disableThiscameraAfterRebind);
        }
    }
}