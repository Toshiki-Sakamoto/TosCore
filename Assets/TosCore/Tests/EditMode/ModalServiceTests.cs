using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using TosCore.UI;
using UnityEngine;

namespace TosCore.Tests.EditMode
{
    public class ModalServiceTests
    {
        [Test]
        public void TryShowAsyncは成功結果を返す()
        {
            var provider = new TestRootProvider();
            var service = new ModalService(provider);
            var builder = new ImmediateSuccessBuilder(42);

            var result = service.TryShowAsync(builder).GetAwaiter().GetResult();

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.EqualTo(42));
            Assert.That(result.IsCanceled, Is.False);
            Assert.That(service.ActiveCount, Is.EqualTo(0));
        }

        [Test]
        public void TryShowAsyncはキャンセル結果を返す()
        {
            var provider = new TestRootProvider();
            var service = new ModalService(provider);
            var builder = new PendingBuilder();

            using var cts = new CancellationTokenSource();
            cts.Cancel();

            var result = service.TryShowAsync(builder, cts.Token).GetAwaiter().GetResult();

            Assert.That(result.IsCanceled, Is.True);
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(service.ActiveCount, Is.EqualTo(0));
        }

        [Test]
        public void CloseTopは最前面モーダルのみ閉じる()
        {
            var provider = new TestRootProvider();
            var service = new ModalService(provider);
            var firstBuilder = new PendingBuilder();
            var secondBuilder = new PendingBuilder();

            var firstTask = service.ShowAsync(firstBuilder);
            var secondTask = service.ShowAsync(secondBuilder);

            service.CloseTop();
            firstBuilder.Presenter.Complete(10);

            Assert.Throws<OperationCanceledException>(() => secondTask.GetAwaiter().GetResult());
            Assert.That(firstTask.GetAwaiter().GetResult(), Is.EqualTo(10));
            Assert.That(service.ActiveCount, Is.EqualTo(0));
        }

        [Test]
        public void CloseAllは全モーダルを閉じる()
        {
            var provider = new TestRootProvider();
            var service = new ModalService(provider);
            var firstBuilder = new PendingBuilder();
            var secondBuilder = new PendingBuilder();

            var firstTask = service.ShowAsync(firstBuilder);
            var secondTask = service.ShowAsync(secondBuilder);

            service.CloseAll();

            Assert.Throws<OperationCanceledException>(() => firstTask.GetAwaiter().GetResult());
            Assert.Throws<OperationCanceledException>(() => secondTask.GetAwaiter().GetResult());
            Assert.That(service.ActiveCount, Is.EqualTo(0));
        }

        [Test]
        public void OpenedとClosedイベントで件数が通知される()
        {
            var provider = new TestRootProvider();
            var service = new ModalService(provider);
            var opened = new List<int>();
            var closed = new List<int>();
            service.Opened += count => opened.Add(count);
            service.Closed += count => closed.Add(count);

            var firstBuilder = new PendingBuilder();
            var secondBuilder = new PendingBuilder();

            var firstTask = service.ShowAsync(firstBuilder);
            var secondTask = service.ShowAsync(secondBuilder);
            service.CloseTop();
            firstBuilder.Presenter.Complete(1);

            Assert.Throws<OperationCanceledException>(() => secondTask.GetAwaiter().GetResult());
            Assert.That(firstTask.GetAwaiter().GetResult(), Is.EqualTo(1));

            Assert.That(opened, Is.EqualTo(new[] { 1, 2 }));
            Assert.That(closed, Is.EqualTo(new[] { 1, 0 }));
        }

        [Test]
        public void ShowAsync完了後に生成Rootが破棄される()
        {
            var provider = new TestRootProvider();
            var service = new ModalService(provider);
            var builder = new ImmediateSuccessBuilder(7);

            _ = service.ShowAsync(builder).GetAwaiter().GetResult();

            Assert.That(builder.CreatedRoot == null, Is.True);
        }

        [Test]
        public void ModalPresenterBaseはOnShow失敗時にOnShowFailedが呼ばれる()
        {
            var presenter = new ThrowOnShowPresenter();

            Assert.Throws<InvalidOperationException>(() => presenter.ShowAsync(CancellationToken.None).GetAwaiter().GetResult());
            Assert.That(presenter.OnShowFailedCalled, Is.True);
        }

        private sealed class TestRootProvider : IModalRootProvider
        {
            private readonly Transform _root;

            public TestRootProvider()
            {
                _root = new GameObject("ModalRoot(Test)").transform;
            }

            public Transform Root => _root;
        }

        private sealed class ImmediateSuccessBuilder : IModalBuilder<int>
        {
            private readonly int _value;

            public ImmediateSuccessBuilder(int value)
            {
                _value = value;
            }

            public Transform CreatedRoot { get; private set; }

            public ModalBuildResult<int> Build(Transform root)
            {
                CreatedRoot = new GameObject("ImmediateModal").transform;
                CreatedRoot.SetParent(root, false);
                return new ModalBuildResult<int>(new ImmediateSuccessPresenter(_value), CreatedRoot);
            }
        }

        private sealed class PendingBuilder : IModalBuilder<int>
        {
            public PendingPresenter Presenter { get; private set; }

            public ModalBuildResult<int> Build(Transform root)
            {
                var modalRoot = new GameObject("PendingModal").transform;
                modalRoot.SetParent(root, false);
                Presenter = new PendingPresenter();
                return new ModalBuildResult<int>(Presenter, modalRoot);
            }
        }

        private sealed class ImmediateSuccessPresenter : IModalPresenter<int>
        {
            private readonly int _value;

            public ImmediateSuccessPresenter(int value)
            {
                _value = value;
            }

            public UniTask<int> ShowAsync(CancellationToken ct)
            {
                return UniTask.FromResult(_value);
            }

            public void Close()
            {
            }
        }

        private sealed class PendingPresenter : IModalPresenter<int>
        {
            private readonly UniTaskCompletionSource<int> _source = new();

            public async UniTask<int> ShowAsync(CancellationToken ct)
            {
                using (ct.Register(() => _source.TrySetCanceled()))
                {
                    return await _source.Task;
                }
            }

            public void Close()
            {
                _source.TrySetCanceled();
            }

            public void Complete(int value)
            {
                _source.TrySetResult(value);
            }
        }

        private sealed class ThrowOnShowPresenter : ModalPresenterBase<int>
        {
            public bool OnShowFailedCalled { get; private set; }

            protected override void OnShow()
            {
                throw new InvalidOperationException("show failed");
            }

            protected override void OnShowFailed(Exception exception)
            {
                OnShowFailedCalled = true;
            }
        }
    }
}
