using System;
using SRXDBackgrounds.Common;
using UnityEngine;

namespace SRXDBackgrounds.Inzo {
    public class Inzo_Pyramid : MonoBehaviour {
        private static readonly int LIGHT_EFFECT_PHASE_1 = Shader.PropertyToID("_Light_Effect_Phase_1");
        private static readonly int LIGHT_EFFECT_PHASE_2 = Shader.PropertyToID("_Light_Effect_Phase_2");
        private static readonly int INTENSITY = Shader.PropertyToID("_Intensity");
        
        [SerializeField] private MeshRenderer pyramidBodyRenderer;
        [SerializeField] private MeshRenderer pyramidRimRenderer;
        [SerializeField] private float lightEffectStartPhase;
        [SerializeField] private float lightEffectEndPhase;
        [SerializeField] private float maxLightEffectDuration;
        [SerializeField] private float rimBaseIntensity;
        [SerializeField] private float rimEffectIntensity;
        [SerializeField] private float rimEffectDuration;

        private EnvelopeBasic lightEffectPhaseEnvelope1;
        private EnvelopeBasic lightEffectPhaseEnvelope2;
        private EnvelopeBasic rimEnvelope;
        private Material pyramidBodyMainMaterial;
        private Material pyramidBodyNotchMaterial;
        private Material pyramidRimMaterial;
        private bool alternateLightEffect;

        private void Awake() {
            lightEffectPhaseEnvelope1 = new EnvelopeBasic();
            lightEffectPhaseEnvelope2 = new EnvelopeBasic();
            rimEnvelope = new EnvelopeBasic { Duration = rimEffectDuration };
            pyramidBodyMainMaterial = pyramidBodyRenderer.materials[0];
            pyramidBodyNotchMaterial = pyramidBodyRenderer.materials[1];
            pyramidRimMaterial = pyramidRimRenderer.material;
        }

        private void LateUpdate() {
            float deltaTime = Time.deltaTime;
            
            pyramidBodyMainMaterial.SetFloat(
                LIGHT_EFFECT_PHASE_1,
                Mathf.Lerp(lightEffectStartPhase, lightEffectEndPhase, lightEffectPhaseEnvelope1.Update(deltaTime)));
            pyramidBodyMainMaterial.SetFloat(
                LIGHT_EFFECT_PHASE_2,
                Mathf.Lerp(lightEffectStartPhase, lightEffectEndPhase, lightEffectPhaseEnvelope2.Update(deltaTime)));

            float rimPhase = rimEnvelope.Update(deltaTime);

            pyramidRimMaterial.SetFloat(INTENSITY, rimBaseIntensity + rimEffectIntensity * (1f - rimPhase * rimPhase * (3f - 2f * rimPhase)));
        }

        public void LightEffect(float duration) {
            duration *= maxLightEffectDuration;
            
            if (alternateLightEffect) {
                lightEffectPhaseEnvelope1.Duration = duration;
                lightEffectPhaseEnvelope1.Trigger();
            }
            else {
                lightEffectPhaseEnvelope2.Duration = duration;
                lightEffectPhaseEnvelope2.Trigger();
            }

            alternateLightEffect = !alternateLightEffect;
        }

        public void RimEffect() => rimEnvelope.Trigger();
    }
}
