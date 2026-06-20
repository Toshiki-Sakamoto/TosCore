using System;
using System.Collections.Generic;
using R3;

namespace TosCore.Input
{
    /// <summary>
    /// <see cref="IInputContextStack"/> の既定実装。top の Action Map だけを Enable し、
    /// それ以外を Disable することでコンテキストを排他制御する。
    /// Input System に触れてよいのはこのスタックと具象サービスまで。
    /// </summary>
    public sealed class InputContextStack : IInputContextStack, IDisposable
    {
        private readonly Stack<IInputContext> _stack = new();
        private readonly ReactiveProperty<IInputContext> _current = new(null);

        /// <inheritdoc />
        public ReadOnlyReactiveProperty<IInputContext> Current => _current;

        /// <inheritdoc />
        public void Push(IInputContext context)
        {
            if (context == null) return;

            // 既存 top をサスペンド。
            if (_stack.Count > 0)
            {
                _stack.Peek().ActionMap.Disable();
            }

            _stack.Push(context);
            context.ActionMap.Enable();
            _current.Value = context;
        }

        /// <inheritdoc />
        public void Pop()
        {
            if (_stack.Count == 0) return;

            var popped = _stack.Pop();
            popped.ActionMap.Disable();

            // 新しい top があれば再開。
            var next = _stack.Count > 0 ? _stack.Peek() : null;
            next?.ActionMap.Enable();
            _current.Value = next;
        }

        /// <inheritdoc />
        public bool Contains(IInputContext context)
        {
            return context != null && _stack.Contains(context);
        }

        public void Dispose()
        {
            foreach (var context in _stack)
            {
                context.ActionMap.Disable();
            }

            _stack.Clear();
            _current.Dispose();
        }
    }
}
