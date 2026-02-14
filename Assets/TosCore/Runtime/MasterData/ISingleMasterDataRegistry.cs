namespace TosCore.MasterData
{
    /// <summary>
    /// 単一マスタデータレジストリインターフェース
    /// </summary>
    public interface ISingleMasterDataRegistry<TMaster> where TMaster : IMasterData<TMaster>
    {
        /// <summary>
        /// マスタの登録
        /// </summary>
        void Register(TMaster master);

        /// <summary>
        /// マスタを削除
        /// </summary>
        void Clear();

        /// <summary>
        /// マスタを取得
        /// </summary>
        TMaster GetValue();
    }
}
