namespace TosCore.Entity
{
    /// <summary>
    /// ファクトリからID割り当てを受けるエンティティ契約
    /// </summary>
    public interface IEntityIdAssignable : IEntity
    {
        void AssignId(IEntityId id);
    }
}
