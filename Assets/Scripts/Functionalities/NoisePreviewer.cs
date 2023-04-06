using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Component that generates a preview image of the noise settings
public class NoisePreviewer : MonoBehaviour
{
    [SerializeField] private RawImage image;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private bool toggle;

    [SerializeField] private WaveSettings settings;

    private float[,] map;

    private void OnValidate() {
        if(toggle){
            image.gameObject.SetActive(true);
        }
        else{
            image.gameObject.SetActive(false);
        }

        GeneratePreview();
    }

    // Generate image
    private void GeneratePreview()
    {
        map = NoiseGenerator.GetPointValues(width, height, settings.scale, settings.heightWaves, settings.offset);

        Color[] pixels = new Color[width * height];
        int i = 0;

        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                pixels[i] = map[x, y] >= settings.threshold ? Color.black : Color.white;
                i++;
            }
        }

        Texture2D tex = new Texture2D(width, height);
        tex.SetPixels(pixels);
        tex.filterMode = FilterMode.Point;
        tex.Apply();

        image.texture = tex;
    }
}
