namespace SRXDBackgrounds.Common {
    public class EnvelopeInverted : EnvelopeBasic {
        protected override float GetValueFromPhase(float phase) => 1f - phase;
    }
}