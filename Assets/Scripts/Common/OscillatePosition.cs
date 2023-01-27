using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SRXDBackgrounds.Common {
    public class OscillatePosition : MonoBehaviour {
        [SerializeField] private Transform root;
        [SerializeField] private Vector3 axis;
        [SerializeField] private float rate;
        [SerializeField] private float initialPhase;

        private float phase;

        private void Awake() {
            phase = initialPhase;
        }

        private void LateUpdate() {
            SetPhase(phase + rate * Time.deltaTime);
            root.localPosition = Mathf.Sin(2f * Mathf.PI * phase) * axis;
        }

        public void SetPhase(float phase) => this.phase = Mathf.Repeat(phase, 1f);

        public void SetAxis(Vector3 axis) => this.axis = axis;
        
        public void SetRate(float rate) => this.rate = rate;

        public void SetInitialPhase(float initialPhase) => this.initialPhase = initialPhase;
    }
}
