using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace TosCore.Scene
{
    /// <summary>
    /// SceneManager で加算ロードする ISubSceneLoader 実装
    /// ロードしたシーンを任意でアクティブにするが、常駐シーン（機構シーン）はアンロードしない
    /// </summary>
    public sealed class SubSceneLoader : ISubSceneLoader
    {
        private readonly Dictionary<string, LoadedSubScene> _loaded = new();

        public IReadOnlyCollection<LoadedSubScene> Loaded => _loaded.Values;

        public event Action<LoadedSubScene> OnLoaded;
        public event Action<SceneReference> OnUnloaded;

        public bool IsLoaded(SceneReference scene)
            => scene.IsValid && _loaded.ContainsKey(scene.Name);

        public async UniTask<LoadedSubScene> LoadAsync(SceneReference scene, bool setActive, CancellationToken ct)
        {
            if (!scene.IsValid) return default;

            // 二重ロード防止。既にロード済みならそれを返す（必要なら再アクティブ化のみ）
            if (_loaded.TryGetValue(scene.Name, out var existing))
            {
                if (setActive && existing.Scene.IsValid())
                {
                    SceneManager.SetActiveScene(existing.Scene);
                }

                return existing;
            }

            var op = SceneManager.LoadSceneAsync(scene.Name, LoadSceneMode.Additive);
            while (op is { isDone: false })
            {
                ct.ThrowIfCancellationRequested();
                await UniTask.Yield();
            }

            var loadedScene = SceneManager.GetSceneByName(scene.Name);
            if (!loadedScene.IsValid()) return default;

            if (setActive)
            {
                SceneManager.SetActiveScene(loadedScene);
            }

            var handle = new LoadedSubScene(scene, loadedScene);
            _loaded[scene.Name] = handle;
            OnLoaded?.Invoke(handle);
            return handle;
        }

        public async UniTask UnloadAsync(SceneReference scene, CancellationToken ct)
        {
            if (!scene.IsValid) return;
            if (!_loaded.Remove(scene.Name, out var handle)) return;

            if (handle.Scene.IsValid() && handle.Scene.isLoaded)
            {
                var op = SceneManager.UnloadSceneAsync(handle.Scene);
                while (op is { isDone: false })
                {
                    ct.ThrowIfCancellationRequested();
                    await UniTask.Yield();
                }
            }
            OnUnloaded?.Invoke(scene);
        }

        public async UniTask<LoadedSubScene> SwapAsync(SceneReference from, SceneReference to, bool setActive, CancellationToken ct)
        {
            await UnloadAsync(from, ct);
            return await LoadAsync(to, setActive, ct);
        }
    }
}
