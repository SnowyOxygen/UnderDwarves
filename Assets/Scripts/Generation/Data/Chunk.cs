using UnityEngine;

public struct Chunk{
    public int[,] walls { get; set; } // 1 == wall, 0 == empty space
    public Vector2Int position { get; set; }
}
