using UnityEngine;

namespace TosCore.UI
{
    public sealed class ModalRootProvider : MonoBehaviour, IModalRootProvider
    {
        [SerializeField] private Transform _root;

        public Transform Root => _root != null ? _root : transform;
    }
}
