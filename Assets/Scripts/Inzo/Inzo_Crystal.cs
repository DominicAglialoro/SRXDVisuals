using System;
using UnityEngine;

namespace SRXDBackgrounds.Inzo {
    public class Inzo_Crystal : MonoBehaviour {
        private static readonly int INTENSITY = Shader.PropertyToID("_Intensity");
        
        [SerializeField] private MeshRenderer innerRenderer;
        [SerializeField] private ParticleSystem particleSystem;

        private Material innerMaterial;

        private void Awake() {
            innerMaterial = innerRenderer.material;
        }

        public void SetIntensity(float intensity) {
            innerMaterial.SetFloat(INTENSITY, intensity);
        }

        public void TriggerParticleSystem() => particleSystem.Play();

        public void DoReset() => particleSystem.Clear();
    }
}