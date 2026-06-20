using UnityEngine.InputSystem;

namespace TosCore.Input
{
    /// <summary>
    /// 入力コンテキストの既定実装。Action Map を1つ提供するだけで、処理は持たない。
    /// スタック（<see cref="IInputContextStack"/>）に積まれると、その Map の有効/無効が排他制御される。
    /// </summary>
    public sealed class InputContext : IInputContext
    {
        /// <inheritdoc />
        public string Id { get; }

        /// <inheritdoc />
        public InputActionMap ActionMap { get; }

        public InputContext(string id, InputActionMap actionMap)
        {
            Id = id;
            ActionMap = actionMap;
        }
    }
}
