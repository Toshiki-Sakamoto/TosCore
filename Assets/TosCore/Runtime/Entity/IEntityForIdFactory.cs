namespace TosCore.Entity
{
    /// <summary>
    /// Entityファクトリ
    /// </summary>
    public interface IEntityForIdFactory<out TEntity>
        where TEntity : IEntity, new()
    {
        TEntity Create();
    }
}