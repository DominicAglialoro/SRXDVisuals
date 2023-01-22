StructuredBuffer<float2> _CustomSpectrumBuffer;

void GetSpectrumValue_float(float at, out float2 Out) {
    at = clamp(at, 0, 255);

    const float2 first = _CustomSpectrumBuffer[floor(at)];
    const float2 second = _CustomSpectrumBuffer[ceil(at)];

    Out = lerp(first, second, frac(at));
}