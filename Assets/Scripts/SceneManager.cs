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
    private string eventIndex = "0";
    private string eventValue = "255.0";

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

        eventIndex = GUILayout.TextField(eventIndex, GUILayout.Width(200f));
        
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Value:");

        eventValue = GUILayout.TextField(eventValue, GUILayout.Width(200f));

        GUILayout.EndHorizontal();
        
        if (GUILayout.Button("Hit")) {
            GetFields(out int index, out float value);
            SendEventHit(index, value);
        }
        
        if (GUILayout.Button("On")) {
            GetFields(out int index, out float value);
            SendEvent(VisualsEventType.On, index, value);
        }
        
        if (GUILayout.Button("Off")) {
            GetFields(out int index, out _);
            SendEvent(VisualsEventType.Off, index);
        }
        
        if (GUILayout.Button("Set Control")) {
            GetFields(out int index, out float value);
            SendEvent(VisualsEventType.ControlChange, index, value);
        }
        
        if (GUILayout.Button("Reset All"))
            VisualsEventManager.Instance.ResetAll();
    }

    private void GetFields(out int index, out float value) {
        if (int.TryParse(eventIndex, out index))
            index = Math.Clamp(index, 0, 255);
        else
            index = 0;

        eventIndex = index.ToString();

        if (float.TryParse(eventValue, out value))
            value = Mathf.Clamp(value, 0f, 255f);
        else
            value = 255f;

        eventValue = value.ToString("0.0#");
    }

    private static void SendEventHit(int index, float value = 255f) {
        var visualsEventManager = VisualsEventManager.Instance;
        
        visualsEventManager.SendEvent(new VisualsEvent(VisualsEventType.On, index, value));
        visualsEventManager.SendEvent(new VisualsEvent(VisualsEventType.Off, index, 255f));
    }
    
    private static void SendEvent(VisualsEventType type, int index, float value = 255f) {
        var visualsEventManager = VisualsEventManager.Instance;
        
        visualsEventManager.SendEvent(new VisualsEvent(type, index, value));
    }

    private static void UpdateNoteHold(ref bool state, bool newState, int index) {
        var visualsEventManager = VisualsEventManager.Instance;
        
        if (!state && newState)
            visualsEventManager.SendEvent(new VisualsEvent(VisualsEventType.On, index, 255f));
        else if (state && !newState)
            visualsEventManager.SendEvent(new VisualsEvent(VisualsEventType.Off, index, 255f));

        state = newState;
    }
}
