using UnityEngine;

namespace SRXDBackgrounds.Common {
    public abstract class EnvelopeNoSustain : IAutomation {
        public float Speed { get; set; }

        public float Duration {
            get => 1f / Speed;
            set => Speed = 1f / value;
        }
        
        private float phase = 1f;
        
        public void Trigger() => phase = 0f;

        public float Update(float deltaTime) {
            phase = Mathf.Min(phase + Speed * deltaTime, 1f);

            return GetValueFromPhase(phase);
        }

        protected abstract float GetValueFromPhase(float phase);
    }
}