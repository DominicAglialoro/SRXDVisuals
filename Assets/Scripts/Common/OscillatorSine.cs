using UnityEngine;

namespace SRXDBackgrounds.Common {
    public class OscillatorSine : Oscillator {
        protected override float GetValueFromPhase(float phase) => Mathf.Sin(2f * Mathf.PI * phase);
    }
}