using System;

namespace TosCore.Entity
{
    /// <summary>
    /// エンティティ基礎
    /// </summary>
    public interface IEntity : IEquatable<IEntity>
    {
        /// <summary>
        /// エンティティ固有ID
        /// </summary>
        IEntityId Id { get; }
        
        /// <summary>
        /// IDによる比較
        /// </summary>
        bool IEquatable<IEntity>.Equals(IEntity other) =>
            other != null && other.Id.Equals(Id);
    }
}
