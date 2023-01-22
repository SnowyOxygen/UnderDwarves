using UnityEngine;
using System;

[Serializable]
public struct OreSettings{
    public Vector2Int veinAmount;
    public Vector2Int veinSize;
    public float veinBias;

    public OreSettings(Vector2Int veinAmount, Vector2Int veinSize, float veinBias)
    {
        this.veinAmount = veinAmount;
        this.veinSize = veinSize;
        this.veinBias = veinBias;
    }
}
