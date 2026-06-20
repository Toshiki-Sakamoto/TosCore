using System;

namespace TosCore.Phase
{
    /// <summary>フェーズ切り替わり時に publish される</summary>
    public readonly struct PhaseChangedMessage<TPhase> where TPhase : Enum
    {
        public readonly TPhase Phase;

        public PhaseChangedMessage(TPhase phase)
        {
            Phase = phase;
        }
    }
}
