using System;
using SRXDCustomVisuals.Core;
using UnityEngine;

namespace SRXDBackgrounds.Inzo {
    public class Inzo_Scene : MonoBehaviour {
        [SerializeField] private Inzo_Pyramid pyramid;
        
        private VisualsEventReceiver eventReceiver;
        
        private void Awake() {
            eventReceiver = GetComponent<VisualsEventReceiver>();
            eventReceiver.On += EventOn;
        }

        private void EventOn(VisualsEvent visualsEvent) {
            switch (visualsEvent.Index) {
                case 0:
                    pyramid.LightEffect((float) (visualsEvent.Value + 1d) / 256f);
                    break;
                case 1:
                    pyramid.RimEffect();
                    break;
            }
        }
    }
}
