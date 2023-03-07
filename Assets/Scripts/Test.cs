using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

public class Test : MonoBehaviour {
    private ComputeBuffer buffer;
    private static readonly int WAVEFORM_CUSTOM = Shader.PropertyToID("_WaveformCustom");

    private void Awake() {
        buffer = new ComputeBuffer(256, UnsafeUtility.SizeOf(typeof(float2)), ComputeBufferType.Default, ComputeBufferMode.SubUpdates);
    }

    private void OnDestroy() {
        buffer.Dispose();
    }

    private void Update() {
        var slice = buffer.BeginWrite<float2>(0, 256);

        for (int i = 0; i < 256; i++) {
            float time = 2f * Mathf.PI * (Time.time + (float) i / 64);

            slice[i] = new float2(Mathf.Cos(time), Mathf.Sin(time));
        }
        
        buffer.EndWrite<float2>(256);
        Shader.SetGlobalBuffer(WAVEFORM_CUSTOM, buffer);
    }
}
