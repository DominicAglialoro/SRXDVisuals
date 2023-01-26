using System;
using SRXDBackgrounds.Common;
using UnityEngine;

namespace SRXDBackgrounds.Inzo {
    public class Inzo_Spotlights : MonoBehaviour {
        private static readonly int INTENSITY = Shader.PropertyToID("_Intensity");
        
        [SerializeField] private Transform[] transforms;
        [SerializeField] private MeshRenderer[] spotlightRenderers;
        [SerializeField] private MeshRenderer[] auraRenderers;
        [SerializeField] private Inzo_Terrain terrain;
        [SerializeField] private Color colorToTerrain;
        [SerializeField] private float defaultAttack;
        [SerializeField] private float defaultDecay;
        [SerializeField] private float defaultSustain;
        [SerializeField] private float defaultRelease;
        [SerializeField] private float maxIntensity;
        [SerializeField] private float oscillatorSpeed;
        [SerializeField] private float defaultOscillatorAmount;

        private Material[] spotlightMaterials;
        private Material[] auraMaterials;
        private EnvelopeADSR[] envelopes;
        private OscillatorSine oscillator;
        private float oscillatorAmount;

        private void Awake() {
            spotlightMaterials = new Material[spotlightRenderers.Length];
            auraMaterials = new Material[auraRenderers.Length];
            envelopes = new EnvelopeADSR[spotlightRenderers.Length];
            oscillator = new OscillatorSine { Speed = oscillatorSpeed };
            oscillatorAmount = defaultOscillatorAmount;
            
            for (int i = 0; i < spotlightRenderers.Length; i++) {
                spotlightMaterials[i] = spotlightRenderers[i].material;
                auraMaterials[i] = auraRenderers[i].material;
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
            float amount = Mathf.Lerp(1f - oscillatorAmount, 1f, oscillator.Update(deltaTime));
            float spotlightIntensity = maxIntensity * amount;
            float sum = 0f;
            
            for (int i = 0; i < envelopes.Length; i++) {
                float value = envelopes[i].Update(deltaTime);

                sum += value;
                value *= spotlightIntensity;
                spotlightMaterials[i].SetFloat(INTENSITY, value);
                auraMaterials[i].SetFloat(INTENSITY, value);
            }

            terrain.SetBackLightColor(amount * sum * colorToTerrain);
        }

        public void Trigger(int index) => envelopes[index].Trigger();

        public void EndSustain(int index) => envelopes[index].EndSustain();

        public void SetOscillatorAmount(float value) => oscillatorAmount = value;

        public void DoReset() {
            foreach (var envelope in envelopes)
                envelope.Reset();

            oscillator.SetPhase(0f);
            oscillatorAmount = defaultOscillatorAmount;
        }
    }
}