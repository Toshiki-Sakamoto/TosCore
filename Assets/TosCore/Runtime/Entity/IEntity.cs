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
        IEntityId Id { get; protected set; }

        /// <summary>
        /// IDの設定
        /// </summary>
        void AssignId(IEntityId id) =>
            Id = id;
        
        /// <summary>
        /// IDによる比較
        /// </summary>
        bool IEquatable<IEntity>.Equals(IEntity other) =>
            other != null && other.Id.Equals(Id);
    }
}