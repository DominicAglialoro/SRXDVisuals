using System;
using SRXDBackgrounds.Common;
using UnityEngine;

namespace SRXDBackgrounds.Inzo {
    public class Inzo_Spotlights : MonoBehaviour {

        [SerializeField] private Inzo_Spotlight[] spotlights;
        [SerializeField] private Inzo_Terrain terrain;
        [SerializeField] private Color colorToTerrain;
        [SerializeField] private Vector3 minBackLightDirection;
        [SerializeField] private Vector3 maxBackLightDirection;
        [SerializeField] private float defaultAttack;
        [SerializeField] private float defaultDecay;
        [SerializeField] private float defaultSustain;
        [SerializeField] private float defaultRelease;
        [SerializeField] private float maxIntensity;
        [SerializeField] private float oscillatorSpeed;
        [SerializeField] private float defaultOscillatorAmount;

        private OscillatorSine oscillator;
        private float oscillatorAmount;

        private void Awake() {
            oscillator = new OscillatorSine { Speed = oscillatorSpeed };
            oscillatorAmount = defaultOscillatorAmount;
            
            foreach (var spotlight in spotlights) {
                var envelope = spotlight.Envelope;

                envelope.Attack = defaultAttack;
                envelope.Decay = defaultDecay;
                envelope.Sustain = defaultSustain;
                envelope.Release = defaultRelease;
            }
        }

        private void LateUpdate() {
            float deltaTime = Time.deltaTime;
            float amount = Mathf.Lerp(1f - oscillatorAmount, 1f, oscillator.Update(deltaTime));
            float spotlightIntensity = maxIntensity * amount;
            float sum = 0f;
            float directionInterp = 0f;

            for (int i = 0; i < spotlights.Length; i++) {
                var spotlight = spotlights[i];
                var envelope = spotlight.Envelope;
                float value = envelope.Update(deltaTime);

                sum += value;
                directionInterp += i * value;
                spotlight.SetIntensity(spotlightIntensity * value);
            }

            if (sum == 0f)
                directionInterp = 0f;
            else
                directionInterp /= sum * (spotlights.Length - 1);

            terrain.SetBackLightColorAndDirection(
                amount * sum * colorToTerrain,
                Vector3.Lerp(minBackLightDirection, maxBackLightDirection, directionInterp));
        }

        public void Trigger(int index) => spotlights[index].Envelope.Trigger();

        public void EndSustain(int index) => spotlights[index].Envelope.EndSustain();

        public void SetOscillatorAmount(float value) => oscillatorAmount = value;

        public void DoReset() {
            foreach (var spotlight in spotlights)
                spotlight.Envelope.Reset();

            oscillator.SetPhase(0f);
            oscillatorAmount = defaultOscillatorAmount;
        }
    }
}