using System.Collections.Generic;
using UnityEngine;

namespace TosCore.TapBlocker
{
    /// <summary>
    /// TapBlockerHandleをMonoBehaviourに紐づけて削除されたときにタップハンドルの解放もしてもらうヘルパークラス
    /// </summary>
    [DisallowMultipleComponent]
    public class TapBlockHandleBinder : MonoBehaviour
    {
        private readonly List<TapBlockHandle> _handles = new();
        private bool _releasing;

        internal static void Bind(GameObject target, TapBlockHandle handle)
        {
            if (target == null) return;

            var binder = target.GetComponent<TapBlockHandleBinder>();
            if (!binder) binder = target.AddComponent<TapBlockHandleBinder>();
            binder.Add(handle);
        }

        private void Add(TapBlockHandle handle)
        {
            if (!handle.IsValid) return;
            _handles.Add(handle);
        }

        private void OnDisable() => 
            ReleaseAll();

        private void OnDestroy() => 
            ReleaseAll();

        private void ReleaseAll()
        {
            if (_releasing) return;
            _releasing = true;

            for (var i = _handles.Count - 1; i >= 0; i--)
            {
                _handles[i].Dispose();
            }

            _handles.Clear();
            _releasing = false;
        }
    }
}