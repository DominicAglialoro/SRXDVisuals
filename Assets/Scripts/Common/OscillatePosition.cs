using UnityEngine;

namespace SRXDBackgrounds.Common {
    public class OscillatePosition : MonoBehaviour {
        [SerializeField] private Transform root;
        [SerializeField] private Vector3 axis;
        [SerializeField] private float initialRate;
        [SerializeField] private float initialPhase;

        public float Rate { get; set; }

        public float Phase {
            get => phase;
            set => phase = Mathf.Repeat(value, 1f);
        }
        
        private float phase;

        private void Awake() {
            Rate = initialRate;
            Phase = initialPhase;
        }

        private void LateUpdate() {
            Phase += Rate * Time.deltaTime;
            root.localPosition = Mathf.Sin(2f * Mathf.PI * Phase) * axis;
        }

#if UNITY_EDITOR
        public void SetAxis(Vector3 axis) => this.axis = axis;
        
        public void SetInitialRate(float rate) => initialRate = rate;

        public void SetInitialPhase(float initialPhase) => this.initialPhase = initialPhase;
#endif
    }
}
