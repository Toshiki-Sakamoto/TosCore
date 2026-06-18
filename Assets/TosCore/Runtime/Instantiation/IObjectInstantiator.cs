using UnityEngine;

namespace TosCore.Instantiation
{
    /// <summary>
    /// Objectを作成するIF
    /// </summary>
    public interface IObjectInstantiator
    {
        GameObject Instantiate(GameObject prefab, Transform parent);

        TComponent Instantiate<TComponent>(TComponent prefab, Transform parent)
            where TComponent : Component;
    }
}
