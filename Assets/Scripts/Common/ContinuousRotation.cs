using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SRXDBackgrounds.Common {
    public class ContinuousRotation : MonoBehaviour {
        [SerializeField] private Transform root;
        [SerializeField] private Vector3 axis;
        [SerializeField] private float rate;

        private void Update() => root.localRotation = Quaternion.AngleAxis(rate * Time.time, axis);
    }
}
