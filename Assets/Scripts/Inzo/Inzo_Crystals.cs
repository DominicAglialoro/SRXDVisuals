using System;
using SRXDBackgrounds.Common;
using UnityEngine;

namespace SRXDBackgrounds.Inzo {
    public class Inzo_Crystals : MonoBehaviour {
        [SerializeField] private Inzo_Crystal[] crystals;
        [SerializeField] private float envelopeDuration;
        [SerializeField] private float maxEnvelopeIntensity;
        [SerializeField] private float maxStarsIntensity;
        [SerializeField] private float maxLightIntensity;
        [SerializeField] private float maxFresnelIntensity;

        private EnvelopeInverted envelope;

        private void Awake() => envelope = new EnvelopeInverted { Duration = envelopeDuration };

        private void Start() => SetIntensity(1f);

        private void LateUpdate() {
            float deltaTime = Time.deltaTime;
            float value = envelope.Update(deltaTime);
            float intensity = maxEnvelopeIntensity * value * value;

            foreach (var crystal in crystals)
                crystal.SetInnerIntensity(intensity);
        }

        public void Trigger() {
            envelope.Trigger();

            foreach (var crystal in crystals)
                crystal.TriggerParticleSystem();
        }

        public void SetIntensity(float value) {
            float starsIntensity = maxStarsIntensity * value;
            float lightIntensity = maxLightIntensity * value;
            float fresnelIntensity = maxFresnelIntensity * value;

            foreach (var crystal in crystals)
                crystal.SetOtherIntensity(starsIntensity, lightIntensity, fresnelIntensity);
        }

        public void DoReset() {
            envelope.Reset();
            SetIntensity(1f);

            foreach (var crystal in crystals)
                crystal.DoReset();
        }
    }
}