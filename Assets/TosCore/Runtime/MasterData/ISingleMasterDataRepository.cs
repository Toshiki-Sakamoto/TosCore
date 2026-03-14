namespace TosCore.MasterData
{
    /// <summary>
    /// 単一マスタデータ参照用リポジトリインターフェース
    /// </summary>
    public interface ISingleMasterDataRepository<out TMaster> where TMaster : IMasterData<TMaster>
    {
        /// <summary>
        /// マスタを取得
        /// </summary>
        TMaster GetValue();
    }
}
