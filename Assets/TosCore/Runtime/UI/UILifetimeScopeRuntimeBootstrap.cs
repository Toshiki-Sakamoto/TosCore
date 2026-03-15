using UnityEngine;

namespace TosCore.UI
{
    /// <summary>
    /// 起動時に１度だけ走る処理
    /// </summary>
    public static class UILifetimeScopeRuntimeBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            var settings = Resources.Load<TosCoreRuntimeSettings>(TosCoreRuntimeSettings.ResourcesPath);
            if (settings == null) return;
            if (settings.UIBootstrapMode != UILifetimeScopeBootstrapMode.AutoCreateIfMissing) return;

            var prefab = settings.UILifetimeScopePrefab;
            if (prefab == null)
            {
                Debug.LogWarning($"[{nameof(UILifetimeScopeRuntimeBootstrap)}] {nameof(TosCoreRuntimeSettings)} has no {nameof(UILifetimeScope)} prefab assigned.");
                return;
            }

            var instance = Object.Instantiate(prefab);
            if (settings.DontDestroyOnLoad)
            {
                Object.DontDestroyOnLoad(instance.gameObject);
            }
        }
    }
}
