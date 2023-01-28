using UnityEngine;

[CreateAssetMenu(fileName = "OreSettings", menuName = "assets/OreSettings", order = 0)]
public class OreSettings : ScriptableObject {
    //TODO: include reference to ore type
    public Sprite tileSprite;
    
    [Header("Generation Settings")]
    public Wave[] heightWaves;
    
    // Vein size & spacing
    public float scale;
}
