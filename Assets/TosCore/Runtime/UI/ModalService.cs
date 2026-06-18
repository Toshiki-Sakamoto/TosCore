using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace TosCore.UI
{
    /// <summary>
    /// モーダルを表示するサービス
    /// </summary>
    public class ModalService : IModalService, IDisposable
    {
        /// <summary>
        /// 表示されている一つのモーダルを管理
        /// </summary>
        private sealed class ModalEntry<TResult> : IModalEntry
        {
            private readonly IModalPresenter<TResult> _presenter;
            private readonly Transform _root;

            public ModalEntry(IModalPresenter<TResult> presenter, Transform root)
            {
                _presenter = presenter;
                _root = root;
            }

            public void Close()
            {
                _presenter.Close();
            }

            public void Destroy()
            {
                if (_root != null)
                {
                    DestroyObject(_root.gameObject);
                }
            }
        }
        
        
        private readonly IModalRootProvider _rootProvider;
        private readonly ModalEntryStack _entryStack = new();

        public event Action<int> Opened;
        public event Action<int> Closed;

        public int ActiveCount => _entryStack.Count;

        [Inject]
        public ModalService(
            IModalRootProvider rootProvider)
        {
            _rootProvider = rootProvider;
        }

        public async UniTask<TResult> ShowAsync<TResult>(IModalBuilder<TResult> builder, CancellationToken ct = default)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var modalRoot = _rootProvider.Root;
            var buildResult = builder.Build(modalRoot);

            var entry = new ModalEntry<TResult>(buildResult.Presenter, buildResult.Root);
            AddEntry(entry);

            try
            {
                return await buildResult.Presenter.ShowAsync(ct);
            }
            finally
            {
                RemoveEntry(entry);
                entry.Destroy();
            }
        }

        public async UniTask<ModalResult<TResult>> TryShowAsync<TResult>(IModalBuilder<TResult> builder, CancellationToken ct = default)
        {
            try
            {
                var result = await ShowAsync(builder, ct);
                return ModalResult<TResult>.Success(result);
            }
            catch (OperationCanceledException) // 外部からCloseが呼び出されてボタン決定じゃないのに進んだ時に呼び出される
            {
                return ModalResult<TResult>.Canceled();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                return ModalResult<TResult>.Faulted(exception);
            }
        }

        public void Show<TResult>(IModalBuilder<TResult> builder, Action<ModalResult<TResult>> onCompleted, CancellationToken ct = default)
        {
            ShowInternalAsync(builder, onCompleted, ct).Forget();
        }

        public void CloseTop()
        {
            if (_entryStack.TryPeek(out var entry))
            {
                entry.Close();
            }
        }

        public void CloseAll()
        {
            var snapshot = _entryStack.SnapshotTopFirst();
            foreach (var t in snapshot)
            {
                t.Close();
            }
        }

        public void Dispose()
        {
            CloseAll();
        }

        private async UniTaskVoid ShowInternalAsync<TResult>(IModalBuilder<TResult> builder, Action<ModalResult<TResult>> onCompleted, CancellationToken ct)
        {
            var result = await TryShowAsync(builder, ct);
            onCompleted?.Invoke(result);
        }

        private void AddEntry(IModalEntry entry)
        {
            _entryStack.Push(entry);
            Opened?.Invoke(_entryStack.Count);
        }

        private void RemoveEntry(IModalEntry entry)
        {
            _entryStack.Remove(entry);
            Closed?.Invoke(_entryStack.Count);
        }
        
        private static void DestroyObject(GameObject target)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEngine.Object.DestroyImmediate(target);
                return;
            }
#endif
            UnityEngine.Object.Destroy(target);
        }
    }
}
