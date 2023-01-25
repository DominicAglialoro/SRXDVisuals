namespace SRXDBackgrounds.Common {
    public class EnvelopeInverted : EnvelopeNoSustain {
        protected override float GetValueFromPhase(float phase) => 1f - phase;
    }
}