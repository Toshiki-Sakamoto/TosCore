using UnityEngine;

namespace TosCore.UI
{
    /// <summary>
    /// モーダルビルダーの結果を格納
    /// </summary>
    public readonly struct ModalBuildResult<TResult>
    {
        public IModalPresenter<TResult> Presenter { get; }

        public Transform Root { get; }

        public ModalBuildResult(IModalPresenter<TResult> presenter, Transform root)
        {
            Presenter = presenter;
            Root = root;
        }
    }
}
