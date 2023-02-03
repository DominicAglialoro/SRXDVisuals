using SRXDBackgrounds.Common;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace SRXDBackgrounds.Inzo {
    [ExecuteInEditMode]
    public class Inzo_CrystalsGenerator : MonoBehaviour {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Camera camera;
        [SerializeField] private int seed;
        [SerializeField] private int countX;
        [SerializeField] private int countZ;
        [SerializeField] private float startMinY;
        [SerializeField] private float startMaxY;
        [SerializeField] private float endMinY;
        [SerializeField] private float endMaxY;
        [SerializeField] private float startZ;
        [SerializeField] private float endZ;
        [SerializeField] private float maxXZVariance;
        [SerializeField] private float minScale;
        [SerializeField] private float maxScale;
        [SerializeField] private float minRotationRate;
        [SerializeField] private float maxRotationRate;
        [SerializeField] private float minOscillateRate;
        [SerializeField] private float maxOscillateRate;
        [SerializeField] private float minOscillateAmount;
        [SerializeField] private float maxOscillateAmount;
        [SerializeField] private bool generate;

#if UNITY_EDITOR
        private void Update() {
            if (!generate)
                return;

            generate = false;
            
            int count = transform.childCount;

            for (int i = 0; i < count; i++)
                DestroyImmediate(transform.GetChild(transform.childCount - 1).gameObject);

            var random = new Random(seed);

            for (int i = 0; i < countZ; i++) {
                float t = (float) i / (countZ - 1);
                float z = Mathf.Lerp(startZ, endZ, t);
                float minX = camera.ViewportToWorldPoint(new Vector3(-1f, 0f, z)).x;
                float maxX = camera.ViewportToWorldPoint(new Vector3(1f, 0f, z)).x;
                float minY = Mathf.Lerp(startMinY, endMinY, t);
                float maxY = Mathf.Lerp(startMaxY, endMaxY, t);
                
                for (int j = 0; j < countX; j++) {
                    float direction = 2f * Mathf.PI * (float) random.NextDouble();
                    var newPosition = new Vector3(
                        Mathf.Lerp(minX, maxX, (float) j / (countX - 1)) + maxXZVariance * Mathf.Cos(direction),
                        Mathf.Lerp(minY, maxY, (float) random.NextDouble()),
                        z + maxXZVariance * Mathf.Sin(direction));
                    var screenPosition = camera.WorldToViewportPoint(newPosition);
                
                    if (screenPosition.x is <= 0f or >= 1f || screenPosition.y is <= 0f or >= 1f)
                        continue;

                    var instance = (GameObject) PrefabUtility.InstantiatePrefab(prefab, transform);
                    var instanceTransform = instance.transform;
                    var continuousRotation = instance.GetComponent<ContinuousRotation>();
                    var oscillatePosition = instance.GetComponent<OscillatePosition>();

                    instanceTransform.position = newPosition;
                    instanceTransform.localScale = Mathf.Lerp(minScale, maxScale, (float) random.NextDouble()) * Vector3.one;
                    // continuousRotation.SetInitialRate((random.Next(2) > 0 ? 1f : -1f) * Mathf.Lerp(minRotationRate, maxRotationRate, (float) random.NextDouble()));
                    // continuousRotation.SetInitialRotation(360f * (float) random.NextDouble());
                    // oscillatePosition.SetAxis(Mathf.Lerp(minOscillateAmount, maxOscillateAmount, (float) random.NextDouble()) * Vector3.up);
                    // oscillatePosition.SetInitialRate(Mathf.Lerp(minOscillateRate, maxOscillateRate, (float) random.NextDouble()));
                    // oscillatePosition.SetInitialPhase((float) random.NextDouble());
                }
            }
        }
#endif
    }
}
