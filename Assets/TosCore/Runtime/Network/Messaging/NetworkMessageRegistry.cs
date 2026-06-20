using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using MessagePipe;

namespace TosCore.Network.Messaging
{
    /// <summary>
    /// メッセージ ID とシリアライザ、受信時 Publish 先（MessagePipe）の対応を保持する。
    /// </summary>
    /// <remarks>
    /// 送信者識別はトランスポート非依存の <see cref="NetworkPlayerId"/> で表す。
    /// </remarks>
    public sealed class NetworkMessageRegistry
    {
        private readonly Dictionary<Type, INetworkMessageRegistration> _registrationsByType = new();
        private readonly Dictionary<ushort, INetworkMessageRegistration> _registrationsById = new();

        /// <summary>
        /// 型に付与された <see cref="NetworkMessageAttribute"/> から MessageId を解決して登録する。
        /// </summary>
        public void Register<TMessage>(INetworkMessageSerializer<TMessage> serializer)
            where TMessage : struct, INetworkMessage
        {
            var messageId = ResolveMessageId<TMessage>();
            var registration = new NetworkMessageRegistration<TMessage>(messageId, serializer);
            _registrationsByType[typeof(TMessage)] = registration;
            _registrationsById[messageId] = registration;
        }

        /// <summary>
        /// シリアライザ登録と同時に MessagePipe の Publisher も紐づける。
        /// </summary>
        public void Register<TMessage>(INetworkMessageSerializer<TMessage> serializer, IPublisher<TMessage> publisher)
            where TMessage : struct, INetworkMessage
        {
            Register(serializer);
            BindPublisher(publisher);
        }

        /// <summary>
        /// 指定 Message 型の受信時 Publish 先を登録する。
        /// </summary>
        public void BindPublisher<TMessage>(IPublisher<TMessage> publisher)
            where TMessage : struct, INetworkMessage
        {
            if (!_registrationsByType.TryGetValue(typeof(TMessage), out var registration))
            {
                throw new InvalidOperationException($"Network message is not registered. Type={typeof(TMessage).FullName}");
            }

            if (registration is not NetworkMessageRegistration<TMessage> typedRegistration)
            {
                throw new InvalidOperationException($"Registered message type mismatch. Type={typeof(TMessage).FullName}");
            }

            typedRegistration.BindPublisher(publisher);
        }

        /// <summary>
        /// メッセージの送信者などの情報をラップした Envelope を生成する。
        /// </summary>
        public NetworkMessageEnvelope CreateEnvelope<TMessage>(in TMessage message)
            where TMessage : struct, INetworkMessage
        {
            if (!_registrationsByType.TryGetValue(typeof(TMessage), out var registration))
            {
                throw new InvalidOperationException($"Network message is not registered. Type={typeof(TMessage).FullName}");
            }

            var typedRegistration = (NetworkMessageRegistration<TMessage>)registration;
            return new NetworkMessageEnvelope(typedRegistration.MessageId, message.Sender, typedRegistration.Serializer.Serialize(message));
        }

        /// <summary>
        /// Payload をデシリアライズして元の Message を復元する。
        /// </summary>
        public TMessage Deserialize<TMessage>(NetworkMessageEnvelope envelope)
            where TMessage : struct, INetworkMessage
        {
            if (!_registrationsById.TryGetValue(envelope.MessageId, out var registration))
            {
                throw new InvalidOperationException($"Network message is not registered. MessageId={envelope.MessageId}");
            }

            if (registration is not NetworkMessageRegistration<TMessage> typedRegistration)
            {
                throw new InvalidOperationException($"Registered message type mismatch. MessageId={envelope.MessageId} Requested={typeof(TMessage).FullName}");
            }

            var message = typedRegistration.Serializer.Deserialize(envelope.Payload);
            message.Sender = envelope.Sender;
            return message;
        }

        /// <summary>
        /// Envelope を復元して登録済み Publisher へ通知する。
        /// </summary>
        public bool TryPublish(NetworkMessageEnvelope envelope)
        {
            if (!_registrationsById.TryGetValue(envelope.MessageId, out var registration))
            {
                return false;
            }

            return registration.TryPublish(envelope);
        }

        private interface INetworkMessageRegistration
        {
            bool TryPublish(NetworkMessageEnvelope envelope);
        }

        /// <summary>
        /// Message 型から ID を解決する。
        /// </summary>
        private static ushort ResolveMessageId<TMessage>()
            where TMessage : struct, INetworkMessage
        {
            var messageType = typeof(TMessage);
            var attribute = messageType.GetCustomAttribute<NetworkMessageAttribute>();
            if (attribute is { HasMessageId: true })
            {
                return attribute.MessageId;
            }

            return CreateMessageIdFromType(messageType);
        }

        /// <summary>
        /// Type から一意な ID を生成する（FNV-1a）。
        /// </summary>
        private static ushort CreateMessageIdFromType(Type messageType)
        {
            var typeName = messageType.FullName ?? messageType.Name;
            var bytes = Encoding.UTF8.GetBytes(typeName);

            const uint fnvOffset = 2166136261;
            const uint fnvPrime = 16777619;

            var hash = fnvOffset;
            for (var i = 0; i < bytes.Length; i++)
            {
                hash ^= bytes[i];
                hash *= fnvPrime;
            }

            var messageId = (ushort)(hash & ushort.MaxValue);
            return messageId == 0 ? (ushort)1 : messageId;
        }

        /// <summary>
        /// メッセージの型毎に必要なシリアライザや内部処理を持つ。
        /// </summary>
        private sealed class NetworkMessageRegistration<TMessage> : INetworkMessageRegistration
            where TMessage : struct, INetworkMessage
        {
            private Action<TMessage> _publish;

            public ushort MessageId { get; }
            public INetworkMessageSerializer<TMessage> Serializer { get; }

            public NetworkMessageRegistration(ushort messageId, INetworkMessageSerializer<TMessage> serializer)
            {
                MessageId = messageId;
                Serializer = serializer;
            }

            public void BindPublisher(IPublisher<TMessage> publisher)
            {
                _publish = publisher.Publish;
            }

            public bool TryPublish(NetworkMessageEnvelope envelope)
            {
                // publisher 未バインドなら何もしない（送信専用メッセージ等）
                if (_publish == null)
                {
                    return false;
                }

                // payload -> Message に変換
                var message = Serializer.Deserialize(envelope.Payload);
                message.Sender = envelope.Sender;
                _publish(message);
                return true;
            }
        }
    }
}
