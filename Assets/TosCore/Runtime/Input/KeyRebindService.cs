using System;
using UnityEngine.InputSystem;
using VContainer;

namespace TosCore.Input
{
    /// <summary>
    /// <see cref="IKeyRebindService"/> の具象実装。
    /// Input System の interactive rebinding と binding override を用いて、
    /// キーコンフィグの変更とその永続化（JSON 保存/読込）を行う。
    /// </summary>
    public sealed class KeyRebindService : IKeyRebindService
    {
        readonly InputActionAsset _asset;

        [Inject]
        public KeyRebindService(InputActionAsset asset)
        {
            _asset = asset;
        }

        public void StartInteractiveRebind(string actionName, int bindingIndex, Action onComplete)
        {
            var action = _asset.FindAction(actionName, throwIfNotFound: true);

            // リバインド中は対象アクションを無効化し、入力が通常処理されないようにする。
            action.Disable();

            action.PerformInteractiveRebinding(bindingIndex)
                .OnComplete(op =>
                {
                    op.Dispose();
                    action.Enable();
                    onComplete?.Invoke();
                })
                .OnCancel(op =>
                {
                    op.Dispose();
                    action.Enable();
                })
                .Start();
        }

        public string ExportOverridesJson()
        {
            return _asset.SaveBindingOverridesAsJson();
        }

        public void ImportOverridesJson(string json)
        {
            _asset.LoadBindingOverridesFromJson(json);
        }

        public void ResetToDefaults()
        {
            _asset.RemoveAllBindingOverrides();
        }
    }
}
