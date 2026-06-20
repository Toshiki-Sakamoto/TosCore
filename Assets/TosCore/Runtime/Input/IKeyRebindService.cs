using System;

namespace TosCore.Input
{
    /// <summary>
    /// キーコンフィグ（リバインド）と、その永続化（binding override の JSON 保存/読込）。
    /// 実装は Input System の interactive rebinding / binding override を用いる。
    /// </summary>
    public interface IKeyRebindService
    {
        /// <summary>指定アクション・バインドの対話的リバインドを開始する（次に押された入力を割当）。</summary>
        void StartInteractiveRebind(string actionName, int bindingIndex, Action onComplete);

        /// <summary>現在の binding override を JSON 化して返す（保存用）。</summary>
        string ExportOverridesJson();

        /// <summary>JSON の binding override を適用する（読込用）。</summary>
        void ImportOverridesJson(string json);

        /// <summary>すべての override を消して既定バインドへ戻す。</summary>
        void ResetToDefaults();
    }
}
