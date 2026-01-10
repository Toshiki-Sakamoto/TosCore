using Cysharp.Threading.Tasks;

namespace TosCore.Scene
{
    /// <summary>
    /// シーン遷移サービス
    /// </summary>
    public interface ISceneTransitionService
    {
        /// <summary>
        /// 指定したシーンに移動する
        /// </summary>
        UniTask LoadSceneAsync(string sceneName);
    }
}
