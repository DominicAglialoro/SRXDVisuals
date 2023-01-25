namespace SRXDBackgrounds.Common {
    public class OscillatorSawtooth : Oscillator {
        protected override float GetValueFromPhase(float phase) => phase;
    }
}