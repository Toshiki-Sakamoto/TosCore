using System;

namespace TosCore.Network.Messaging
{
    /// <summary>
    /// ネットワーク越しに送りたいメッセージ構造体に付与する Attribute。
    /// </summary>
    /// <remarks>
    /// 現状で意味を持つのは MessageId の固定指定のみ（SourceGenerator は未提供）。
    /// （MessageId 未指定時は型名から FNV-1a ハッシュで自動採番する。AutoRelay は予約フラグ）
    /// </remarks>
    [AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class NetworkMessageAttribute : Attribute
    {
        public NetworkMessageAttribute()
        {
            HasMessageId = false;
            MessageId = default;
            AutoRelay = true;
        }

        public NetworkMessageAttribute(bool autoRelay)
        {
            HasMessageId = false;
            MessageId = default;
            AutoRelay = autoRelay;
        }

        public NetworkMessageAttribute(ushort messageId)
        {
            HasMessageId = true;
            MessageId = messageId;
            AutoRelay = true;
        }

        public NetworkMessageAttribute(ushort messageId, bool autoRelay)
        {
            HasMessageId = true;
            MessageId = messageId;
            AutoRelay = autoRelay;
        }

        public bool HasMessageId { get; }
        public ushort MessageId { get; }
        public bool AutoRelay { get; }
    }
}
