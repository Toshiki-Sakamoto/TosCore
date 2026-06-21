using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace TosCore.Scene
{
    /// <summary>
    /// 常駐シーンを残したまま追加シーンを加算ロード／アンロード／差し替えする
    /// 前のシーンを捨てる画面遷移用の ISceneLoader とは責務が異なる
    /// </summary>
    public interface ISubSceneLoader
    {
        /// <summary>ロード済みサブシーンの一覧</summary>
        IReadOnlyCollection<LoadedSubScene> Loaded { get; }

        /// <summary>指定サブシーンがロード済みか</summary>
        bool IsLoaded(SceneReference scene);

        /// <summary>サブシーンを加算ロードする。setActive=true でアクティブシーンにする（常駐シーンは残す）</summary>
        UniTask<LoadedSubScene> LoadAsync(SceneReference scene, bool setActive, CancellationToken ct);

        /// <summary>サブシーンをアンロードする（未ロードなら何もしない）</summary>
        UniTask UnloadAsync(SceneReference scene, CancellationToken ct);

        /// <summary>from をアンロードして to をロードする（サブシーンの差し替え）</summary>
        UniTask<LoadedSubScene> SwapAsync(SceneReference from, SceneReference to, bool setActive, CancellationToken ct);

        /// <summary>ロード完了時に発火する（ロード後のシーン内処理のフックに使う）</summary>
        event Action<LoadedSubScene> OnLoaded;

        /// <summary>アンロード完了時に発火する</summary>
        event Action<SceneReference> OnUnloaded;
    }
}
