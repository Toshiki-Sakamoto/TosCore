using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace TosCore.UI
{
    /// <summary>
    /// モーダル専用の基底クラス
    /// </summary>
    public abstract class ModalPresenterBase<TResult> : IModalPresenter<TResult>
    {
        private UniTaskCompletionSource<TResult> _completionSource;
        private bool _isShowing;
        private bool _isClosed;

        public async UniTask<TResult> ShowAsync(CancellationToken ct)
        {
            if (_isShowing)
            {
                throw new InvalidOperationException("Modal is already showing.");
            }

            _completionSource = new UniTaskCompletionSource<TResult>();
            _isShowing = true;
            _isClosed = false;

            await using (ct.Register(CancelByToken))
            {
                try
                {
                    if (!ct.IsCancellationRequested)
                    {
                        await ShowViewAsync(ct);
                    }

                    return await _completionSource.Task;
                }
                catch (Exception exception)
                {
                    OnShowFailed(exception);
                    throw;
                }
                finally
                {
                    _completionSource = null;
                    _isShowing = false;
                    _isClosed = false;
                }
            }
        }

        public void Close()
        {
            if (_completionSource == null || _isClosed) return;

            _isClosed = true;
            _completionSource.TrySetCanceled();
            CloseView();
        }

        protected void SetResult(TResult result)
        {
            if (_completionSource == null || _isClosed) return;

            _isClosed = true;
            _completionSource.TrySetResult(result);
            CloseView();
        }

        /// <summary>
        /// Viewの表示待機処理
        /// </summary>
        protected abstract UniTask ShowViewAsync(CancellationToken ct);

        protected virtual void OnShowFailed(Exception exception)
        {
        }

        protected virtual void CloseView()
        {
        }

        private void CancelByToken()
        {
            Close();
        }
    }
}
