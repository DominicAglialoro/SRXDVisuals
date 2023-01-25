using UnityEngine;

namespace SRXDBackgrounds.Common {
    public abstract class Oscillator : IAutomation {
        public float Speed { get; set; }

        public float Duration {
            get => 1f / Speed;
            set => Speed = 1f / value;
        }
        
        private float phase;

        public void SetPhase(float phase) => this.phase = phase;
        
        public float Update(float deltaTime) {
            phase = Mathf.Repeat(phase + Speed * deltaTime, 1f);

            return GetValueFromPhase(phase);
        }
        
        protected abstract float GetValueFromPhase(float phase);
    }
}