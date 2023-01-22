using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk{
    public int[,] walls; // 1 == wall, 0 == empty space
    public Vector2Int position;

    private Chunk(){}
    public Chunk(int chunkSize, NoiseGenerator noise, Vector2Int position){
        walls = new int[chunkSize, chunkSize];
        this.position = position;

        for(int x = 0; x < chunkSize; x++){
            for (int y = 0; y < chunkSize; y++)
            {
                float value = noise.GetPointValue(
                    position.x * chunkSize + x,
                    position.y * chunkSize + y
                );

                walls[x, y] = (value >= 0) ? 1 : 0; 
            }
        }
    }
    public static Chunk EmptyChunk(int chunkSize, Vector2Int pos) => new Chunk(){
        walls = new int[chunkSize, chunkSize],
        position = pos
    };
}
