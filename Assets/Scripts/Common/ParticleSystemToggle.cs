using UnityEngine;

namespace SRXDBackgrounds.Common {
    public class ParticleSystemToggle : MonoBehaviour {
        [SerializeField] private ParticleSystem[] particleSystems;

        public void Play() {
            foreach (var particleSystem in particleSystems)
                particleSystem.Play();
        }
        
        public void Stop() {
            foreach (var particleSystem in particleSystems)
                particleSystem.Stop();
        }

        public void SetEmission(bool enable) {
            foreach (var particleSystem in particleSystems) {
                var emission = particleSystem.emission;

                emission.enabled = enable;
            }
        }
    }
}
