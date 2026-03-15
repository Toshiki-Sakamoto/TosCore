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

            using (ct.Register(CancelByToken))
            {
                try
                {
                    if (!ct.IsCancellationRequested)
                    {
                        OnShow();
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
            OnClose();
        }

        protected void SetResult(TResult result)
        {
            if (_completionSource == null || _isClosed) return;

            _isClosed = true;
            _completionSource.TrySetResult(result);
            OnClose();
        }

        protected abstract void OnShow();

        protected virtual void OnShowFailed(Exception exception)
        {
        }

        protected virtual void OnClose()
        {
        }

        private void CancelByToken()
        {
            Close();
        }
    }
}
