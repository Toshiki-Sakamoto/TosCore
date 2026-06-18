using UnityEngine;

namespace TosCore.Instantiation
{
    public sealed class UnityObjectInstantiator : IObjectInstantiator
    {
        public GameObject Instantiate(GameObject prefab, Transform parent)
        {
            return Object.Instantiate(prefab, parent);
        }

        public TComponent Instantiate<TComponent>(TComponent prefab, Transform parent)
            where TComponent : Component
        {
            return Object.Instantiate(prefab, parent);
        }
    }
}
