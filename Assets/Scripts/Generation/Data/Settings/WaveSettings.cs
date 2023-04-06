using UnityEngine;

[CreateAssetMenu(fileName = "WaveSettings", menuName = "UnderDwarves/WaveSettings", order = 2)]

// Preset class for different noise settings
public class WaveSettings : ScriptableObject {
    public Vector2 offset;
    public float scale;
    public float threshold;
    public Wave[] heightWaves;
}
