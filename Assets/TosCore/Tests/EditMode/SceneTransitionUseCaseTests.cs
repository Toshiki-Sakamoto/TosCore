using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using TosCore.Scene;

namespace TosCore.Tests.EditMode
{
    public class SceneTransitionUseCaseTests
    {
        [Test]
        public void ExecuteAsyncでライフサイクル終了後にシーンロードする()
        {
            var calls = new List<string>();
            var lifecycleRunner = new TestSceneLifecycleRunner(calls);
            var loader = new TestSceneLoader(calls);
            var useCase = new SceneTransitionUseCase(lifecycleRunner, loader);

            useCase.ExecuteAsync("Battle").GetAwaiter().GetResult();

            Assert.That(calls, Is.EqualTo(new[] { "shutdown", "load:Battle" }));
        }

        [Test]
        public void シーン名が空白のときは何もしない()
        {
            var calls = new List<string>();
            var lifecycleRunner = new TestSceneLifecycleRunner(calls);
            var loader = new TestSceneLoader(calls);
            var useCase = new SceneTransitionUseCase(lifecycleRunner, loader);

            useCase.ExecuteAsync(" ").GetAwaiter().GetResult();

            Assert.That(calls, Is.Empty);
        }

        private sealed class TestSceneLifecycleRunner : ISceneLifecycleRunner
        {
            private readonly IList<string> _calls;

            public TestSceneLifecycleRunner(IList<string> calls)
            {
                _calls = calls;
            }

            public SceneLifecycleState State => SceneLifecycleState.Updating;

            public void PostInitialize()
            {
            }

            public void Tick()
            {
            }

            public UniTask StartAsync(CancellationToken cancellation)
            {
                return UniTask.CompletedTask;
            }

            public UniTask ShutdownAsync()
            {
                _calls.Add("shutdown");
                return UniTask.CompletedTask;
            }

            public void Dispose()
            {
            }
        }

        private sealed class TestSceneLoader : ISceneLoader
        {
            private readonly IList<string> _calls;

            public TestSceneLoader(IList<string> calls)
            {
                _calls = calls;
            }

            public UniTask LoadSceneAsync(string sceneName)
            {
                _calls.Add($"load:{sceneName}");
                return UniTask.CompletedTask;
            }
        }
    }
}
