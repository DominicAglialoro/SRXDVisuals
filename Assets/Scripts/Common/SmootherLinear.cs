using UnityEngine;

namespace SRXDBackgrounds.Common {
    public class SmootherLinear : Smoother {
        public float RateUpward { get; set; }
        
        public float RateDownward { get; set; }
        
        protected override float GetNewValue(float value, float target, float deltaTime)
            => Mathf.MoveTowards(value, target, (target >= value ? RateUpward : RateDownward) * deltaTime);
    }
}