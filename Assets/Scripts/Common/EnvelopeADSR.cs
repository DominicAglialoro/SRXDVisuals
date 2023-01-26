using UnityEngine;

namespace SRXDBackgrounds.Common {
    public class EnvelopeADSR {
        public float Attack { get; set; }
        
        public float Decay { get; set; }
        
        public float Sustain { get; set; }
        
        public float Release { get; set; }

        private float phase = 3f;
        private bool sustained;

        public void Trigger() {
            phase = 0f;
            sustained = true;
        }

        public void EndSustain() {
            sustained = false;
        }

        public void Reset() {
            phase = 3f;
            sustained = false;
        }

        public float Update(float deltaTime) {
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

            if (phase < 2f) {
                if (Decay <= 0f)
                    phase = 2f;
                else {
                    float remaining = Decay * (2f - phase);

                    if (deltaTime < remaining) {
                        phase += deltaTime / Decay;

                        return Mathf.Lerp(1f, Sustain, phase - 1f);
                    }

                    phase = 2f;
                    deltaTime -= remaining;
                }
            }

            if (sustained) {
                phase = 2f;

                return Sustain;
            }

            if (Release <= 0 || phase >= 3f) {
                phase = 3f;

                return 0f;
            }

            phase = Mathf.Min(phase + deltaTime / Release, 3f);

            return Mathf.Lerp(Sustain, 0f, phase - 2f);
        }
    }
}