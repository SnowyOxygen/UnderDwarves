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

    [Header("Preview Settings")]
    public RawImage previewImage;
    public int previewWidth;
    public int previewHeight;
    public bool togglePreview = false;

    [Header("Wall Settings")]
    public Vector2 offset;
    public Wave[] heightWaves;
    public float scale;
    public int chunkSize;
    [Range(0, 1)] public float threshold = 0.5f;

    [Header("Ore settings")]
    public List<OreSettings> oreSettings = new List<OreSettings>();

    // Variables
    private float [,] previewMap;
    private NoiseGenerator noise;
    private Dictionary<Vector2Int, Chunk> chunks = new Dictionary<Vector2Int, Chunk>();

    private void Start() {
        noise = new NoiseGenerator(scale, heightWaves, offset);

        ChunkBuilder cBuilder = new ChunkBuilder(noise, chunkSize);
        chunks.Add(Vector2Int.zero, cBuilder.GetEmptyChunk(Vector2Int.zero));
    }

    #region Preview
    private void OnValidate() {
        if(chunkSize < 0 || (chunkSize & (chunkSize - 1)) != 0){
            Debug.LogWarning("Chunk size is not a power of 8");
        }

        if(togglePreview){
            if(!previewImage.gameObject.activeSelf) previewImage.gameObject.SetActive(true);
            GeneratePreview();
        }
        else{
            if(previewImage.gameObject.activeSelf) previewImage.gameObject.SetActive(false);
        }
        GeneratePreview();
    }
    private void GeneratePreview(){
        previewMap = NoiseGenerator.GetPointValues(previewWidth, previewHeight, scale, heightWaves, offset);

        Color[] pixels = new Color[previewWidth * previewHeight];
        int i = 0;

        for(int x = 0; x < previewWidth; x++){
            for (int y = 0; y < previewHeight; y++)
            {
                pixels[i] = previewMap[x, y] >= threshold ? Color.black : Color.white;
                i++;
            }
        }

        Texture2D tex = new Texture2D(previewWidth, previewHeight);
        tex.SetPixels(pixels);
        tex.filterMode = FilterMode.Point;
        tex.Apply();

        previewImage.texture = tex;
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
        ChunkBuilder chunkBuilder = new ChunkBuilder(noise, chunkSize);

        if(chunks.ContainsKey(position)){
            return chunks[position];
        }
        else{
            Chunk newChunk = chunkBuilder.GetChunk(position, threshold);
            chunks.Add(position, newChunk);
            return newChunk;
        }
    }
}