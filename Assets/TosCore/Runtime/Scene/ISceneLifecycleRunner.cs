using System;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace TosCore.Scene
{
    public interface ISceneLifecycleRunner : IInitializable, ITickable, IAsyncStartable, IDisposable
    {
        SceneLifecycleState State { get; }
        UniTask ShutdownAsync();
    }
}
