using UnityEngine;

public class ChunkBuilder
{
    private NoiseGenerator _noise;
    private int _size;

    public ChunkBuilder(NoiseGenerator noise, int size){
        _noise = noise;
        _size = size;
    }

    public Chunk GetChunk(Vector2Int position){
        int[,] walls = new int[_size, _size];

        for(int x = 0; x < _size; x++){
            for (int y = 0; y < _size; y++)
            {
                float value = _noise.GetPointValue(
                    position.x * _size + x,
                    position.y * _size + y
                );

                walls[x, y] = (value >= 0) ? 1 : 0; 
            }
        }

        return new Chunk(){
            walls = walls,
            position = position
        };
    }

    public Chunk GetEmptyChunk(Vector2Int position){
        return new Chunk(){
            walls = new int[_size, _size],
            position = position
        };
    }
}
