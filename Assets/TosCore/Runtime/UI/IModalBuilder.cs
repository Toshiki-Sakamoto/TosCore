using UnityEngine;

namespace TosCore.UI
{
    /// <summary>
    /// モーダル専用ビルダークラス
    /// </summary>
    public interface IModalBuilder<TResult>
    {
        ModalBuildResult<TResult> Build(Transform root);
    }
}
