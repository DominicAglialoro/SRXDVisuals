using UnityEngine;

namespace SRXDBackgrounds.Common {
    public class ContinuousRotation : MonoBehaviour {
        [SerializeField] private Transform root;
        [SerializeField] private Vector3 axis;
        [SerializeField] private float initialRate;
        [SerializeField] private float initialRotation;

        public float Rate { get; set; }

        public float Rotation {
            get => rotation;
            set => rotation = rotation = Mathf.Repeat(value, 360f);
        }
        
        private float rotation;

        private void Awake() {
            Rate = initialRate;
            Rotation = initialRotation;
        }

        private void LateUpdate() {
            Rotation += Rate * Time.deltaTime;
            root.localRotation = Quaternion.AngleAxis(Rotation, axis);
        }

#if UNITY_EDITOR
        public void SetInitialRate(float rate) => this.initialRate = rate;
        
        public void SetInitialRotation(float initialRotation) => this.initialRotation = initialRotation;
#endif
    }
}
