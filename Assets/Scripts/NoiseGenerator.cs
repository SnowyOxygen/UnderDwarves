using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator{
    private float scale;
    private Wave[] waves;
    private Vector2 offset;

    public NoiseGenerator(float scale, Wave[] waves, Vector2 offset){
        this.scale = scale;
        this.waves = waves;
        this.offset = offset;
    }
    public float GetPointValue(int x, int y){
        return GetValue(x, y, this.scale, this.offset, this.waves);
    }
    private static float GetValue(int x, int y, float scale, Vector2 offset, Wave[] waves){
                float samplePosX = (float)x * scale + offset.x;
                float samplePosY = (float)y * scale + offset.y;

                float normalization = 0.0f;

                float value = 0f;

                foreach(Wave wave in waves){
                    float waveSamplePosX = samplePosX * wave.frequency + wave.seed;
                    float waveSamplePosY = samplePosY * wave.frequency + wave.seed;

                    value += wave.amplitude * Mathf.PerlinNoise(waveSamplePosX, waveSamplePosY);
                    normalization += wave.amplitude;
                }

                value /= normalization;

                return value;
    }
    public static float[,] GetPointValues(int width, int height,
    float scale, Wave[] waves, Vector2 offset){
        float[,] noiseMap = new float[width, height];

        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                noiseMap[x, y] = GetValue(
                    x, y, scale, offset, waves
                );
            }
        }

        return noiseMap;
    }
}

[System.Serializable]
public class Wave{
    public float seed;
    public float frequency;
    public float amplitude;
}
