using System;

namespace TosCore.UI
{
    /// <summary>
    /// モーダルの結果を返すResultパターン
    /// </summary>
    public readonly struct ModalResult<TResult>
    {
        public bool IsSuccess { get; }

        public bool IsCanceled { get; }

        public bool IsFaulted => Exception != null;

        public TResult Value { get; }

        public Exception Exception { get; }

        
        private ModalResult(bool isSuccess, bool isCanceled, TResult value, Exception exception)
        {
            IsSuccess = isSuccess;
            IsCanceled = isCanceled;
            Value = value;
            Exception = exception;
        }

        public static ModalResult<TResult> Success(TResult value) =>
            new ModalResult<TResult>(true, false, value, null);

        public static ModalResult<TResult> Canceled() =>
            new ModalResult<TResult>(false, true, default, null);

        public static ModalResult<TResult> Faulted(Exception exception) =>
            new ModalResult<TResult>(false, false, default, exception);
    }
}
