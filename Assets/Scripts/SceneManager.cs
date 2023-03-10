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
    private float eventValue;
    private string eventIndexField = "0";
    private string eventValueField = "256.0";

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

        eventIndexField = GUILayout.TextField(eventIndexField, GUILayout.Width(200f));
        
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Value:");

        eventValueField = GUILayout.TextField(eventValueField, GUILayout.Width(200f));

        GUILayout.EndHorizontal();
        
        if (GUILayout.Button("Hit")) {
            GetFields();
            SendEventHit(eventIndex, eventValue);
        }
        
        if (GUILayout.Button("On")) {
            GetFields();
            SendEvent(VisualsEventType.On, eventIndex, eventValue);
        }
        
        if (GUILayout.Button("Off")) {
            GetFields();
            SendEvent(VisualsEventType.Off, eventIndex);
        }
        
        if (GUILayout.Button("Set Control")) {
            GetFields();
            SendEvent(VisualsEventType.ControlChange, eventIndex, eventValue);
        }
        
        if (GUILayout.Button("Reset All"))
            VisualsEventManager.ResetAll();
    }

    private void GetFields() {
        if (int.TryParse(eventIndexField, out int index))
            eventIndex = Math.Clamp(index, 0, 255);

        eventIndexField = eventIndex.ToString();

        if (float.TryParse(eventValueField, out float value))
            eventValue = Mathf.Clamp(value, 0f, 256f);

        eventValueField = eventValue.ToString("0.0#");
    }

    private static void SendEventHit(int index, float value = 255f) {
        VisualsEventManager.SendEvent(new VisualsEvent(VisualsEventType.On, index, value));
        VisualsEventManager.SendEvent(new VisualsEvent(VisualsEventType.Off, index, 255f));
    }
    
    private static void SendEvent(VisualsEventType type, int index, float value = 255f)
        => VisualsEventManager.SendEvent(new VisualsEvent(type, index, value));

    private static void UpdateNoteHold(ref bool state, bool newState, int index) {
        if (!state && newState)
            VisualsEventManager.SendEvent(new VisualsEvent(VisualsEventType.On, index, 255f));
        else if (state && !newState)
            VisualsEventManager.SendEvent(new VisualsEvent(VisualsEventType.Off, index, 255f));

        state = newState;
    }
}
