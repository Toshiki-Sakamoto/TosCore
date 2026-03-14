using System;

namespace TosCore.Entity
{
    /// <summary>
    /// EntityID基底インターフェース
    /// </summary>
    public interface IEntityId : IEquatable<IEntityId>, IIdentifier
    {
    }
}