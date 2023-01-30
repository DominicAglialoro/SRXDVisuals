using UnityEngine;

namespace SRXDBackgrounds.Common {
    public class EnvelopeADSR {
        public float Attack { get; set; }
        
        public float Decay { get; set; }
        
        public float Sustain { get; set; }
        
        public float Release { get; set; }

        private float phase = 2f;
        private float releasePhase = 1f;
        private bool sustained;

        public void Trigger() {
            phase = 0f;
            releasePhase = 0f;
            sustained = true;
        }

        public void EndSustain() {
            releasePhase = 0f;
            sustained = false;
        }

        public void Reset() {
            phase = 2f;
            releasePhase = 1f;
            sustained = false;
        }

        public float Update(float deltaTime) => Mathf.Lerp(UpdateAttackDecay(deltaTime), 0f, UpdateRelease(deltaTime));

        private float UpdateAttackDecay(float deltaTime) {
            if (phase < 1f) {
                if (Attack <= 0f)
                    phase = 1f;
                else {
                    float remaining = Attack * (1f - phase);

                    if (deltaTime <= remaining) {
                        phase += deltaTime / Attack;

                        return phase;
                    }

                    phase = 1f;
                    deltaTime -= remaining;
                }
            }

            if (phase >= 2f || Decay <= 0f) {
                phase = 2f;

                return Sustain;
            }

            phase += deltaTime / Decay;

            if (phase < 2f)
                return Mathf.Lerp(1f, Sustain, phase - 1f);
            
            phase = 2f;

            return Sustain;
        }

        private float UpdateRelease(float deltaTime) {
            if (sustained) {
                releasePhase = 0f;

                return 0f;
            }
            
            if (releasePhase >= 1f || Release <= 0f) {
                releasePhase = 1f;

                return 1f;
            }

            releasePhase += deltaTime / Release;

            if (releasePhase < 1f)
                return releasePhase;
            
            releasePhase = 1f;

            return 1f;
        }
    }
}