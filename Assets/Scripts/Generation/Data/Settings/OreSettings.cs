using UnityEngine;

[CreateAssetMenu(fileName = "OreSettings", menuName = "assets/OreSettings", order = 0)]
public class OreSettings : ScriptableObject {
    //TODO: include reference to ore type
    public Sprite tileSprite;
    
    [Header("Generation Settings")]
    /// <summary>
    /// Maximum amount of tiles to be placed
    /// </summary>
    public Vector2Int veinSize;
    /// <summary>
    /// Maximum amount of veins that can appear in a chunk
    /// </summary>
    public int maxVeinAmount;
    /// <summary>
    /// Probability for each vein to spawn
    /// </summary>
    public float probability;
}
