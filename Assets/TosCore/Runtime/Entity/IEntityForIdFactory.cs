namespace TosCore.Entity
{
    /// <summary>
    /// Entityファクトリ
    /// </summary>
    public interface IEntityForIdFactory<out TEntity>
        where TEntity : IEntityIdAssignable, new()
    {
        TEntity Create();
    }
}
