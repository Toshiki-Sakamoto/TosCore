using TosCore.UI;
using UnityEngine;

namespace TosCore
{
    public enum UILifetimeScopeBootstrapMode
    {
        Disabled = 0,
        Manual = 1,
        AutoCreateIfMissing = 2
    }

    [CreateAssetMenu(fileName = "TosCoreRuntimeSettings", menuName = "TosCore/Runtime Settings")]
    public sealed class TosCoreRuntimeSettings : ScriptableObject
    {
        public const string ResourcesPath = "TosCoreRuntimeSettings";

        [Header("UI設定")]
        [SerializeField] private UILifetimeScopeBootstrapMode _uiBootstrapMode = UILifetimeScopeBootstrapMode.Manual;
        [SerializeField] private UILifetimeScope _uiLifetimeScopePrefab;
        [SerializeField] private bool _dontDestroyOnLoad = true;

        public UILifetimeScopeBootstrapMode UIBootstrapMode => _uiBootstrapMode;
        public UILifetimeScope UILifetimeScopePrefab => _uiLifetimeScopePrefab;
        public bool DontDestroyOnLoad => _dontDestroyOnLoad;
    }
}
