namespace TosCore.Entity
{
    /// <summary>
    /// 型ごとの連番採番サービス
    /// </summary>
    public interface IIdSequenceService
    {
        /// <summary>
        /// 次のT型のIDを取り出す
        /// </summary>
        long NextFor<T>();
        
        /// <summary>
        /// 次のT型のIDを取り出す
        /// </summary>
        long PeekFor<T>();
    }
}
