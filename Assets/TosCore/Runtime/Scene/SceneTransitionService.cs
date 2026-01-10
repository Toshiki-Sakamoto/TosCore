using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace TosCore.Scene
{
    public class SceneTransitionService : ISceneTransitionService
    {
        public async UniTask LoadSceneAsync(string sceneName)
        {
            // 現在のアクティブシーンを取得
            var currentScene = SceneManager.GetActiveScene();
            
            // 新しいシーンを追加で読み込み
            var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (op is { isDone: false }) await UniTask.Yield();
            
            // 新しいシーンをアクティブに設定
            var newScene = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(newScene);
            
            // 前のシーンをアンロード
            if (/*currentScene.name != "" && */currentScene.IsValid())
            {
                SceneManager.UnloadSceneAsync(currentScene);
            }
        }
    }
}
