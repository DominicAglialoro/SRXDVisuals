using System;
using SRXDBackgrounds.Common;
using SRXDCustomVisuals.Core;
using UnityEngine;

namespace SRXDBackgrounds.Inzo {
    public class Inzo_Scene : MonoBehaviour {
        [SerializeField] private Inzo_Pyramid pyramid;
        [SerializeField] private Inzo_Terrain terrain;
        [SerializeField] private ParticleSystemToggle sparkles;
        
        private VisualsEventReceiver eventReceiver;
        
        private void Awake() {
            eventReceiver = GetComponent<VisualsEventReceiver>();
            eventReceiver.On += EventOn;
            eventReceiver.Off += EventOff;
        }

        private void EventOn(VisualsEvent visualsEvent) {
            switch (visualsEvent.Index) {
                case 0:
                    pyramid.LightEffect((float) (visualsEvent.Value + 1d) / 256f);
                    break;
                case 1:
                    pyramid.RimEffect();
                    break;
                case 2:
                    terrain.Wave();
                    break;
                case 3:
                    sparkles.SetEmission(true);
                    break;
            }
        }

        private void EventOff(VisualsEvent visualsEvent) {
            switch (visualsEvent.Index) {
                case 3:
                    sparkles.SetEmission(false);
                    break;
            }
        }
    }
}
