using Cysharp.Threading.Tasks;

namespace TosCore.Scene
{
    /// <summary>
    /// シーンロード処理
    /// </summary>
    public interface ISceneLoader
    {
        /// <summary>
        /// 指定したシーンに移動する
        /// </summary>
        UniTask LoadSceneAsync(string sceneName);
    }
}
