using UnityEngine;

[CreateAssetMenu(fileName = "WaveSettings", menuName = "UnderDwarves/WaveSettings", order = 2)]
public class WaveSettings : ScriptableObject {
    public Vector2 offset;
    public float scale;
    public float threshold;
    public Wave[] heightWaves;
}
