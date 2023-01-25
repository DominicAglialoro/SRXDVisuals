using System;
using System.Collections;
using System.Collections.Generic;
using SRXDBackgrounds.Common;
using UnityEngine;

namespace SRXDBackgrounds.Inzo {
    public class Inzo_Terrain : MonoBehaviour {
        private static readonly int WAVE_PHASE = Shader.PropertyToID("_Wave_Phase");
        
        [SerializeField] private MeshRenderer terrainRenderer;
        [SerializeField] private float waveStartDistance;
        [SerializeField] private float waveEndDistance;
        [SerializeField] private float waveDuration;
        
        private EnvelopeBasic wavePhaseEnvelope;
        private Material terrainMaterial;

        private void Awake() {
            wavePhaseEnvelope = new EnvelopeBasic() { Duration = waveDuration };
            terrainMaterial = terrainRenderer.material;
        }

        private void LateUpdate() {
            float deltaTime = Time.deltaTime;
            
            terrainMaterial.SetFloat(WAVE_PHASE, Mathf.Lerp(waveStartDistance, waveEndDistance, wavePhaseEnvelope.Update(deltaTime)));
        }

        public void Wave() => wavePhaseEnvelope.Trigger();
    }
}
