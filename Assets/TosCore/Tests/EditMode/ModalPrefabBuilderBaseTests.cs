using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using TosCore.Instantiation;
using TosCore.UI;
using UnityEngine;

namespace TosCore.Tests.EditMode
{
    public class ModalPrefabBuilderBaseTests
    {
        [Test]
        public void BuildはInstantiator経由でViewを生成してResultを返す()
        {
            var root = new GameObject("ModalRoot(Test)").transform;
            var prefabGo = new GameObject("ModalView(Prefab)");
            var prefab = prefabGo.AddComponent<TestModalView>();

            try
            {
                var instantiator = new RecordingInstantiator();
                var builder = new TestBuilder(instantiator, prefab);

                var result = builder.Build(root);
                var presenter = result.Presenter as TestPresenter;

                Assert.That(instantiator.InstantiateComponentCallCount, Is.EqualTo(1));
                Assert.That(instantiator.LastParent, Is.EqualTo(root));
                Assert.That(result.Root, Is.EqualTo(instantiator.CreatedView.transform));
                Assert.That(presenter, Is.Not.Null);
                Assert.That(presenter.View, Is.EqualTo(instantiator.CreatedView));
            }
            finally
            {
                Object.DestroyImmediate(root.gameObject);
                Object.DestroyImmediate(prefabGo);
            }
        }

        private sealed class TestModalView : MonoBehaviour
        {
        }

        private sealed class TestBuilder : ModalPrefabBuilderBase<int, TestModalView>
        {
            public TestBuilder(IObjectInstantiator instantiator, TestModalView prefab)
                : base(instantiator, prefab)
            {
            }

            protected override IModalPresenter<int> CreatePresenter(TestModalView view)
            {
                return new TestPresenter(view);
            }
        }

        private sealed class TestPresenter : IModalPresenter<int>
        {
            public TestModalView View { get; }

            public TestPresenter(TestModalView view)
            {
                View = view;
            }

            public UniTask<int> ShowAsync(CancellationToken ct)
            {
                return UniTask.FromResult(0);
            }

            public void Close()
            {
            }
        }

        private sealed class RecordingInstantiator : IObjectInstantiator
        {
            public int InstantiateComponentCallCount { get; private set; }
            public Transform LastParent { get; private set; }
            public TestModalView CreatedView { get; private set; }

            public GameObject Instantiate(GameObject prefab, Transform parent)
            {
                var clone = Object.Instantiate(prefab, parent);
                return clone;
            }

            public TComponent Instantiate<TComponent>(TComponent prefab, Transform parent)
                where TComponent : Component
            {
                InstantiateComponentCallCount++;
                LastParent = parent;

                var go = new GameObject("ModalView(Instance)");
                go.transform.SetParent(parent, false);
                CreatedView = go.AddComponent<TestModalView>();
                return CreatedView as TComponent;
            }
        }
    }
}
