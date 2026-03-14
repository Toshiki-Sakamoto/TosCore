using Cysharp.Threading.Tasks;
using VContainer;

namespace TosCore.Scene
{
    public sealed class SceneTransitionUseCase : ISceneTransitionUseCase
    {
        private readonly ISceneLifecycleRunner _sceneLifecycleRunner;
        private readonly ISceneLoader _sceneLoader;

        [Inject]
        public SceneTransitionUseCase(
            ISceneLifecycleRunner sceneLifecycleRunner,
            ISceneLoader sceneLoader)
        {
            _sceneLifecycleRunner = sceneLifecycleRunner;
            _sceneLoader = sceneLoader;
        }

        public async UniTask ExecuteAsync(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName)) return;

            try
            {
                await _sceneLifecycleRunner.ShutdownAsync();
                await _sceneLoader.LoadSceneAsync(sceneName);
            }
            finally
            {
            }
        }
    }
}
