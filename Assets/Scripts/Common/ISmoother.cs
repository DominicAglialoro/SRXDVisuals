namespace SRXDBackgrounds.Common {
    public interface ISmoother {
        float Update(float target, float deltaTime);
    }
}