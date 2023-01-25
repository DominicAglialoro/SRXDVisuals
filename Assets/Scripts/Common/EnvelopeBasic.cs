namespace SRXDBackgrounds.Common {
    public class EnvelopeBasic : EnvelopeNoSustain {
        protected override float GetValueFromPhase(float phase) => phase;
    }
}