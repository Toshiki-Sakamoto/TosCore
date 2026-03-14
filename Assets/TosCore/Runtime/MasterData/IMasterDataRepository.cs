using System.Collections.Generic;

namespace TosCore.MasterData
{
    /// <summary>
    /// マスタデータ参照用リポジトリインターフェース
    /// </summary>
    public interface IMasterDataRepository<TMaster> where TMaster : IMasterData<TMaster>
    {
        /// <summary>
        /// マスタをIDから検索
        /// </summary>
        bool TryGetValue(MasterId<TMaster> id, out TMaster master);

        /// <summary>
        /// 全マスタを取得
        /// </summary>
        IEnumerable<TMaster> GetAllMasters();
    }
}
