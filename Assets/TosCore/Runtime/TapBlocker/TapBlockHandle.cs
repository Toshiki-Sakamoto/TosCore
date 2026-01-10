using System;
using UnityEngine;

namespace TosCore.TapBlocker
{
    public readonly struct TapBlockHandle : IDisposable
    {
        private readonly TapBlockerService _owner;
        private readonly int _requestId;

        internal TapBlockHandle(TapBlockerService owner, int requestId)
        {
            _owner = owner;
            _requestId = requestId;
        }
    
        public bool IsValid => _owner != null && _requestId != 0;

        public void Dispose()
        {
            _owner?.Release(_requestId);
        }

        public TapBlockHandle BindTo(GameObject target)
        {
            if (!IsValid || target == null) return this;
            TapBlockHandleBinder.Bind(target, this);
            return this;
        }
    
        public TapBlockHandle BindTo(MonoBehaviour behaviour) =>
            BindTo(behaviour ? behaviour.gameObject : null);
    }

}