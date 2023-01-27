using System;
using UnityEngine;

namespace SRXDBackgrounds.Common {
    public class ContinuousRotation : MonoBehaviour {
        [SerializeField] private Transform root;
        [SerializeField] private Vector3 axis;
        [SerializeField] private float rate;
        [SerializeField] private float initialRotation;

        private float rotation;

        private void Awake() {
            rotation = initialRotation;
        }

        private void LateUpdate() {
            SetRotation(rotation + rate * Time.deltaTime);
            root.localRotation = Quaternion.AngleAxis(rotation, axis);
        }

        public void SetRotation(float rotation) => this.rotation = Mathf.Repeat(rotation, 360f);

        public void SetInitialRotation(float initialRotation) => this.initialRotation = initialRotation;

        public void SetRate(float rate) => this.rate = rate;
    }
}
