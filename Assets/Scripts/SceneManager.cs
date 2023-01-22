using System;
using SRXDCustomVisuals.Core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SceneManager : MonoBehaviour {
    [SerializeField] private GameObject gameplayOverlay;
    [SerializeField] private Volume postProcessingVolume;

    private bool showGameplayOverlay = true;
    private bool enablePostProcessing = true;
    private float bloom = 0.5f;
    private Bloom bloomComponent;
    private bool holding;
    private bool beatHolding;
    private bool spinningRight;
    private bool spinningLeft;
    private bool scratching;
    private int eventIndex;
    private double eventValue = 255d;

    private void Start() {
        postProcessingVolume.profile.TryGet(out bloomComponent);
    }

    private void OnGUI() {
        GUILayout.Label("Visuals");
        showGameplayOverlay = GUILayout.Toggle(showGameplayOverlay, "Show Gameplay Overlay");
        enablePostProcessing = GUILayout.Toggle(enablePostProcessing, "Enable Post-Processing");
        GUILayout.Label("Bloom");
        bloom = GUILayout.HorizontalSlider(bloom, 0f, 1f);
        
        gameplayOverlay.SetActive(showGameplayOverlay);
        postProcessingVolume.gameObject.SetActive(enablePostProcessing);

        if (bloomComponent != null)
            bloomComponent.intensity.value = bloom;
        
        GUILayout.Space(20f);
        GUILayout.Label("Note Events");

        if (GUILayout.Button("Hit Match"))
            SendEventHit((int) NoteIndex.HitMatch);
        
        if (GUILayout.Button("Hit Tap"))
            SendEventHit((int) NoteIndex.HitTap);
        
        if (GUILayout.Button("Hit Beat"))
            SendEventHit((int) NoteIndex.HitBeat);
        
        if (GUILayout.Button("Hit Spin Right"))
            SendEventHit((int) NoteIndex.HitSpinRight);
        
        if (GUILayout.Button("Hit Spin Left"))
            SendEventHit((int) NoteIndex.HitSpinLeft);
        
        if (GUILayout.Button("Hit Scratch"))
            SendEventHit((int) NoteIndex.HitScratch);
        
        UpdateNoteHold(ref holding, GUILayout.Toggle(holding, "Holding"), (int) NoteIndex.Hold);
        UpdateNoteHold(ref beatHolding, GUILayout.Toggle(beatHolding, "Beat Holding"), (int) NoteIndex.HoldBeat);
        UpdateNoteHold(ref spinningRight, GUILayout.Toggle(spinningRight, "Spinning Right"), (int) NoteIndex.HoldSpinRight);
        UpdateNoteHold(ref spinningLeft, GUILayout.Toggle(spinningLeft, "Spinning Left"), (int) NoteIndex.HoldSpinLeft);
        UpdateNoteHold(ref scratching, GUILayout.Toggle(scratching, "Scratching"), (int) NoteIndex.HoldScratch);
        
        GUILayout.Space(20f);
        GUILayout.Label("Other Events");
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Index:");

        string field = GUILayout.TextField(eventIndex.ToString(), GUILayout.Width(200f));

        if (string.IsNullOrWhiteSpace(field))
            eventIndex = 0;
        else if (int.TryParse(field, out int newEventIndex))
            eventIndex = Mathf.Clamp(newEventIndex, 0, 255);
        
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Value:");

        field = GUILayout.TextField(eventValue.ToString("0.0#"), GUILayout.Width(200f));
        
        if (string.IsNullOrWhiteSpace(field))
            eventValue = 0d;
        else if (double.TryParse(field, out double newEventValue))
            eventValue = Math.Clamp(newEventValue, 0d, 255d);
        
        GUILayout.EndHorizontal();
        
        if (GUILayout.Button("Hit"))
            SendEventHit(eventIndex, eventValue);
        
        if (GUILayout.Button("On"))
            SendEvent(VisualsEventType.On, eventIndex, eventValue);
        
        if (GUILayout.Button("Off"))
            SendEvent(VisualsEventType.Off, eventIndex);
        
        if (GUILayout.Button("Set Control"))
            SendEvent(VisualsEventType.ControlChange, eventIndex, eventValue);
    }

    private static void SendEventHit(int index, double value = 255d) {
        var visualsEventManager = VisualsEventManager.Instance;
        
        visualsEventManager.SendEvent(new VisualsEvent(VisualsEventType.On, index, value));
        visualsEventManager.SendEvent(new VisualsEvent(VisualsEventType.Off, index, 255d));
    }
    
    private static void SendEvent(VisualsEventType type, int index, double value = 255d) {
        var visualsEventManager = VisualsEventManager.Instance;
        
        visualsEventManager.SendEvent(new VisualsEvent(type, index, value));
    }

    private static void UpdateNoteHold(ref bool state, bool newState, int index) {
        var visualsEventManager = VisualsEventManager.Instance;
        
        if (!state && newState)
            visualsEventManager.SendEvent(new VisualsEvent(VisualsEventType.On, index, 255d));
        else if (state && !newState)
            visualsEventManager.SendEvent(new VisualsEvent(VisualsEventType.Off, index, 255d));

        state = newState;
    }
}
