using System;
using SRXDCustomVisuals.Core;
using UnityEngine;

namespace SRXDBackgrounds.Inzo {
    public class Inzo_Scene : MonoBehaviour {
        [SerializeField] private Inzo_Spotlights spotlights;
        [SerializeField] private Inzo_Pyramid pyramid;
        [SerializeField] private Inzo_Sparkles sparkles;
        [SerializeField] private Inzo_Terrain terrain;
        
        private VisualsEventReceiver eventReceiver;
        
        private void Awake() {
            eventReceiver = GetComponent<VisualsEventReceiver>();
            eventReceiver.On += EventOn;
            eventReceiver.Off += EventOff;
            eventReceiver.ControlChange += EventControlChange;
            eventReceiver.Reset += EventReset;
        }

        private void EventOn(VisualsEvent visualsEvent) {
            int index = visualsEvent.Index;
            float value = visualsEvent.Value;
            float valueNormalized = value / 255f;
            
            switch (index) {
                case < 8:
                    spotlights.Trigger(index);
                    break;
                case 8:
                    pyramid.LightEffect(valueNormalized);
                    break;
                case 9:
                    pyramid.RimEffect();
                    break;
                case 10:
                    sparkles.Play();
                    break;
                case 11:
                    terrain.Wave();
                    break;
            }
        }

        private void EventOff(VisualsEvent visualsEvent) {
            int index = visualsEvent.Index;
            float value = visualsEvent.Value;
            float valueNormalized = value / 255f;
            
            switch (index) {
                case < 8:
                    spotlights.EndSustain(index);
                    break;
                case 10:
                    sparkles.Stop();
                    break;
            }
        }

        private void EventControlChange(VisualsEvent visualsEvent) {
            int index = visualsEvent.Index;
            float value = visualsEvent.Value;
            float valueNormalized = value / 255f;
            
            switch (index) {
                case 0:
                    spotlights.SetOscillatorAmount(valueNormalized);
                    break;
                case 1:
                    spotlights.SetOscillatorSpeed(valueNormalized);
                    break;
            }
        }

        private void EventReset() {
            spotlights.DoReset();
            pyramid.DoReset();
            sparkles.DoReset();
            terrain.DoReset();
        }
    }
}
