using System;
using SRXDBackgrounds.Common;
using UnityEngine;

namespace SRXDBackgrounds.Inzo {
    public class Inzo_Spotlights : MonoBehaviour {
        private static readonly int INTENSITY = Shader.PropertyToID("_Intensity");
        
        [SerializeField] private Transform[] transforms;
        [SerializeField] private MeshRenderer[] renderers;
        [SerializeField] private float defaultAttack;
        [SerializeField] private float defaultDecay;
        [SerializeField] private float defaultSustain;
        [SerializeField] private float defaultRelease;
        [SerializeField] private float maxIntensity;

        private Material[] materials;
        private EnvelopeADSR[] envelopes;

        private void Awake() {
            materials = new Material[renderers.Length];
            envelopes = new EnvelopeADSR[renderers.Length];
            
            for (int i = 0; i < renderers.Length; i++) {
                materials[i] = renderers[i].material;
                envelopes[i] = new EnvelopeADSR() {
                    Attack = defaultAttack,
                    Decay = defaultDecay,
                    Sustain = defaultSustain,
                    Release = defaultRelease
                };
            }
        }

        private void LateUpdate() {
            float deltaTime = Time.deltaTime;
            
            for (int i = 0; i < envelopes.Length; i++) {
                float value = envelopes[i].Update(deltaTime);

                materials[i].SetFloat(INTENSITY, maxIntensity * value);
            }
        }

        public void Trigger(int index) => envelopes[index].Trigger();

        public void EndSustain(int index) => envelopes[index].EndSustain();
    }
}