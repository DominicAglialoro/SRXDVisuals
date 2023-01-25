namespace SRXDBackgrounds.Common {
    public abstract class SmootherBasic : ISmoother {
        private float value;

        public void SetValue(float value) => this.value = value;
        
        public float Update(float target, float deltaTime) {
            value = GetNewValue(value, target, deltaTime);

            return value;
        }

        protected abstract float GetNewValue(float value, float target, float deltaTime);
    }
}