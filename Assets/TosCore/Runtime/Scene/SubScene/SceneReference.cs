namespace TosCore.Scene
{
    /// <summary>
    /// サブシーンの参照
    /// 現状はシーン名ベース。将来 build index / Addressables アドレスへ拡張する余地を残す
    /// </summary>
    public readonly struct SceneReference
    {
        public string Name { get; }

        public bool IsValid => !string.IsNullOrEmpty(Name);

        private SceneReference(string name) => Name = name;

        /// <summary>シーン名から参照を作る</summary>
        public static SceneReference FromName(string sceneName) => new (sceneName);

        public override string ToString() => IsValid ? Name : "(invalid SceneReference)";
    }
}
