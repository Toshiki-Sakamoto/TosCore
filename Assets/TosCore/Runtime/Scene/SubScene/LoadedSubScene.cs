using UnityScene = UnityEngine.SceneManagement.Scene;

namespace TosCore.Scene
{
    /// <summary>
    /// ロード済みサブシーンのハンドル
    /// アンロードや、ロード後のシーン内オブジェクト走査の足がかりに使う
    /// </summary>
    public readonly struct LoadedSubScene
    {
        public SceneReference Reference { get; }
        public UnityScene Scene { get; }

        public LoadedSubScene(SceneReference reference, UnityScene scene)
        {
            Reference = reference;
            Scene = scene;
        }

        public bool IsValid => Reference.IsValid && Scene.IsValid();
    }
}
