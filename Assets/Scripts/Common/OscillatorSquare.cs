namespace SRXDBackgrounds.Common {
    public class OscillatorSquare : Oscillator {
        protected override float GetValueFromPhase(float phase) => phase < 0.5f ? 1f : 0f;
    }
}