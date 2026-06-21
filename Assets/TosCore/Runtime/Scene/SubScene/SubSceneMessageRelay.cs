using System;
using MessagePipe;
using VContainer.Unity;

namespace TosCore.Scene
{
    /// <summary>
    /// ISubSceneLoader の C# イベントを MessagePipe へ中継するブリッジ
    /// SubSceneLoader 本体を MessagePipe 非依存（依存最小）に保ちつつ、
    /// MessagePipe を使う構成ではこれを登録するだけで Loaded/Unloaded が publish される
    /// </summary>
    public sealed class SubSceneMessageRelay : IStartable, IDisposable
    {
        private readonly ISubSceneLoader _loader;
        private readonly IPublisher<SubSceneLoadedMessage> _loadedPublisher;
        private readonly IPublisher<SubSceneUnloadedMessage> _unloadedPublisher;

        public SubSceneMessageRelay(
            ISubSceneLoader loader,
            IPublisher<SubSceneLoadedMessage> loadedPublisher,
            IPublisher<SubSceneUnloadedMessage> unloadedPublisher)
        {
            _loader = loader;
            _loadedPublisher = loadedPublisher;
            _unloadedPublisher = unloadedPublisher;
        }

        public void Start()
        {
            _loader.OnLoaded += HandleLoaded;
            _loader.OnUnloaded += HandleUnloaded;
        }

        public void Dispose()
        {
            _loader.OnLoaded -= HandleLoaded;
            _loader.OnUnloaded -= HandleUnloaded;
        }

        private void HandleLoaded(LoadedSubScene scene)
            => _loadedPublisher.Publish(new SubSceneLoadedMessage(scene));

        private void HandleUnloaded(SceneReference scene)
            => _unloadedPublisher.Publish(new SubSceneUnloadedMessage(scene));
    }
}
