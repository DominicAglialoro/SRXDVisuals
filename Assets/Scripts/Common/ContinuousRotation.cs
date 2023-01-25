using UnityEngine;

namespace SRXDBackgrounds.Common {
    public class ContinuousRotation : MonoBehaviour {
        [SerializeField] private Transform root;
        [SerializeField] private Vector3 axis;
        [SerializeField] private float rate;

        private float rotation;

        private void LateUpdate() {
            SetRotation(rotation + rate * Time.deltaTime);
            root.localRotation = Quaternion.AngleAxis(rotation, axis);
        }

        public void SetRotation(float rotation) => this.rotation = Mathf.Repeat(rotation, 360f);
    }
}
