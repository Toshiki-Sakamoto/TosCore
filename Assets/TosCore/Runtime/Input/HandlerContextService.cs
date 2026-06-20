using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace TosCore.Input
{
    /// <summary>
    /// 離散入力（押下）を、登録された <typeparamref name="THandler"/> 群へ配送する
    /// <see cref="InputContextService"/> の基底。ハンドラの登録/解除と配送ループをここに集約する。
    /// 具象は <c>OnTick</c> で <see cref="Dispatch"/> を並べるだけでよい。
    /// </summary>
    public abstract class HandlerContextService<THandler> : InputContextService
    {
        private readonly List<THandler> _handlers = new();

        protected HandlerContextService(IInputContext context) : base(context)
        {
        }

        /// <summary>ハンドラを登録。返り値を Dispose すると解除される。</summary>
        public IDisposable Register(THandler handler)
        {
            _handlers.Add(handler);
            return new Registration(this, handler);
        }

        /// <summary>
        /// <paramref name="action"/> が今フレーム押されていれば、登録ハンドラへ
        /// 逆順（後勝ち）で <paramref name="invoke"/> を配送する。
        /// <paramref name="invoke"/> を static ラムダにすればアロケーションは出ない。
        /// </summary>
        protected void Dispatch(InputAction action, Action<THandler> invoke)
        {
            if (action == null || !action.WasPressedThisFrame()) return;
            for (var i = _handlers.Count - 1; i >= 0; i--)
            {
                invoke(_handlers[i]);
            }
        }

        public override void Dispose()
        {
            _handlers.Clear();
        }

        private sealed class Registration : IDisposable
        {
            private HandlerContextService<THandler> _owner;
            private THandler _handler;

            public Registration(HandlerContextService<THandler> owner, THandler handler)
            {
                _owner = owner;
                _handler = handler;
            }

            public void Dispose()
            {
                if (_owner == null) return;
                _owner._handlers.Remove(_handler);
                _owner = null;
                _handler = default;
            }
        }
    }
}
