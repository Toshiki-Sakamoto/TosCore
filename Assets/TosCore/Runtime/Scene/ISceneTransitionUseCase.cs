using Cysharp.Threading.Tasks;

namespace TosCore.Scene
{
    /// <summary>
    /// シーン遷移ユースケース
    /// </summary>
    public interface ISceneTransitionUseCase
    {
        UniTask ExecuteAsync(string sceneName);
    }
}
