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
        [SerializeField] private float maxOscillatorAmount;
        [SerializeField] private float spread;

        private OscillatorSine oscillator;
        private float oscillatorAmount;

        private void Awake() {
            oscillator = new OscillatorSine { Speed = oscillatorSpeed };
            
            foreach (var spotlight in spotlights) {
                var envelope = spotlight.Envelope;

                envelope.Attack = defaultAttack;
                envelope.Decay = defaultDecay;
                envelope.Sustain = defaultSustain;
                envelope.Release = defaultRelease;
            }
            
            SetAngle(1f);
        }

        private void LateUpdate() {
            float deltaTime = Time.deltaTime;
            float intensityScale = Mathf.Lerp(1f - oscillatorAmount, 1f, oscillator.Update(deltaTime));
            float spotlightIntensity = maxIntensity * intensityScale;
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
                intensityScale * Mathf.Log(sum + 1f) * colorToTerrain,
                Vector3.Lerp(minBackLightDirection, maxBackLightDirection, directionInterp));
        }

        public void Trigger(int index) => spotlights[index].Envelope.Trigger();

        public void EndSustain(int index) => spotlights[index].Envelope.EndSustain();

        public void SetOscillatorAmount(float value) => oscillatorAmount = maxOscillatorAmount * value;

        public void SetAngle(float value) {
            for (int i = 0; i < spotlights.Length; i++) {
                var spotlight = spotlights[i];
                float min = i >= spotlights.Length / 2 ? 90f : -90f;
                float max = Mathf.Lerp(-spread, spread, (float) i / (spotlights.Length - 1));
                
                spotlight.SetRotation(Mathf.Lerp(min, max, value));
            }
        }

        public void DoReset() {
            foreach (var spotlight in spotlights)
                spotlight.Envelope.Reset();

            oscillator.SetPhase(0f);
            oscillatorAmount = 0f;
            SetAngle(1f);
        }
    }
}