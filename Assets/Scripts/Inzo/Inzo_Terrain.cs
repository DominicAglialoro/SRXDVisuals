using System.Collections.Generic;
using SRXDBackgrounds.Common;
using UnityEngine;

namespace SRXDBackgrounds.Inzo {
    public class Inzo_Terrain : MonoBehaviour {
        private static readonly int WAVE_PHASE = Shader.PropertyToID("_Wave_Phase");
        private static readonly int MIDDLE_LIGHT_COLOR = Shader.PropertyToID("_Middle_Light_Color");
        
        [SerializeField] private MeshRenderer terrainRenderer;
        [SerializeField] private float waveStartDistance;
        [SerializeField] private float waveEndDistance;
        [SerializeField] private float waveDuration;
        
        private EnvelopeBasic wavePhaseEnvelope;
        private Material terrainMaterial;
        private Dictionary<string, Color> middleLightSources;

        private void Awake() {
            wavePhaseEnvelope = new EnvelopeBasic() { Duration = waveDuration };
            terrainMaterial = terrainRenderer.material;
            middleLightSources = new Dictionary<string, Color>();
        }

        private void LateUpdate() {
            float deltaTime = Time.deltaTime;
            
            terrainMaterial.SetFloat(WAVE_PHASE, Mathf.Lerp(waveStartDistance, waveEndDistance, wavePhaseEnvelope.Update(deltaTime)));
        }

        public void Wave() => wavePhaseEnvelope.Trigger();

        public void SetMiddleLightSource(string name, Color color) {
            middleLightSources[name] = color;
            
            var middleLightColor = Color.black;

            foreach (var value in middleLightSources.Values)
                middleLightColor += value;
            
            terrainMaterial.SetColor(MIDDLE_LIGHT_COLOR, middleLightColor);
        }
    }
}
