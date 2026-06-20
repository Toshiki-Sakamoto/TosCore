namespace TosCore.Network.Messaging
{
    /// <summary>
    /// ネットワーク越しに受信した Envelope の通知（MessagePipe で流す内部メッセージ）。
    /// </summary>
    public readonly struct NetworkMessageEnvelopeReceivedMessage
    {
        public NetworkMessageEnvelopeReceivedMessage(NetworkMessageEnvelope envelope)
        {
            Envelope = envelope;
        }

        public NetworkMessageEnvelope Envelope { get; }
    }
}
