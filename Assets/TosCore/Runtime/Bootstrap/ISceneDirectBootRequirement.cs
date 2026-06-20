namespace TosCore.Bootstrap
{
    /// <summary>
    /// シーン直起動時に満たしておきたい前提条件。
    /// PJ が実装し、SceneLifetimeScope のスコープに登録する（複数可）。
    /// </summary>
    public interface ISceneDirectBootRequirement
    {
        /// <summary>チェック項目名（ログ・デバッグ用）。</summary>
        string Label { get; }

        /// <summary>条件を満たしているか判定する。</summary>
        bool IsSatisfied();
    }
}
