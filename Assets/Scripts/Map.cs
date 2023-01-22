using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    #region Singleton
    public static Map instance;

    private void Awake() {
        if(instance == null){
            instance = this;
        }
        else{
            Debug.LogError("More than one instance of Map!");
            Destroy(gameObject);
        }
    }
    #endregion

    // Components
    public RawImage debugImage;

    [Header("Dimensions")]
    public int width;
    public int height;
    public float scale;
    public Vector2 offset;

    [Header("Height Map")]
    public Wave[] heightWaves;
    public bool toggleDebug = false;

    [Header("Chunk Size")] 
    public int chunkSize;

    // Variables
    private float [,] heightMap;
    private NoiseGenerator noise;

    private Dictionary<Vector2Int, Chunk> chunks = new Dictionary<Vector2Int, Chunk>();

    private void Start() {
        noise = new NoiseGenerator(scale, heightWaves, offset);

        chunks.Add(Vector2Int.zero, Chunk.EmptyChunk(chunkSize, Vector2Int.zero));
    }

    #region Preview
    private void OnValidate() {
        if(chunkSize < 0 || (chunkSize & (chunkSize - 1)) != 0){
            Debug.LogWarning("Chunk size is not a power of 8");
        }

        if(toggleDebug){
            if(!debugImage.gameObject.activeSelf) debugImage.gameObject.SetActive(true);
            GeneratePreview();
        }
        else{
            if(debugImage.gameObject.activeSelf) debugImage.gameObject.SetActive(false);
        }
        GeneratePreview();
    }
    private void GeneratePreview(){
        heightMap = NoiseGenerator.GetPointValues(width, height, scale, heightWaves, offset);

        Color[] pixels = new Color[width * height];
        int i = 0;

        for(int x = 0; x < width; x++){
            for (int y = 0; y < height; y++)
            {
                pixels[i] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
                i++;
            }
        }

        Texture2D tex = new Texture2D(width, height);
        tex.SetPixels(pixels);
        tex.filterMode = FilterMode.Point;
        tex.Apply();

        debugImage.texture = tex;
    }
    #endregion
    
    public Vector2Int WorldToChunkPos(Vector2 pos){
        int x = Mathf.RoundToInt(pos.x / chunkSize);
        int y = Mathf.RoundToInt(pos.y / chunkSize);

        return new Vector2Int(x, y);
    }

    // Generation
    // Get chunks if exist, generate if not
    public Chunk[] GetChunks(List<Vector2Int> toFind){
        Chunk[] chunksToGet = new Chunk[toFind.Count];

        for(int i = 0; i < toFind.Count; i++){
            // Check if chunk exists
            Vector2Int position = toFind[i];

            chunksToGet[i] = GetChunk(position);
        }

        return chunksToGet;
    }

    public Chunk GetChunk(Vector2Int position){
        if(chunks.ContainsKey(position)){
            return chunks[position];
        }
        else{
            Chunk newChunk = new Chunk(chunkSize, noise, position);
            chunks.Add(position, newChunk);
            return newChunk;
        }
    }
}