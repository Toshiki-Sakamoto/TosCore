using UnityEngine;

namespace TosCore
{
    [DisallowMultipleComponent]
    public sealed class DontDestroyOnLoadComponent : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
