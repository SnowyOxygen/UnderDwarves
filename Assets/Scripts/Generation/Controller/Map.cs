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

    [SerializeField] private WaveSettings wallSettings;

    public int chunkSize;

    // Variables
    private NoiseGenerator noise;
    private Dictionary<Vector2Int, Chunk> chunks = new Dictionary<Vector2Int, Chunk>();

    private void Start() {
        noise = new NoiseGenerator(wallSettings);

        ChunkBuilder cBuilder = new ChunkBuilder(noise, chunkSize);
        chunks.Add(Vector2Int.zero, cBuilder.GetEmptyChunk(Vector2Int.zero));
    }

    private void OnValidate() {
        if(chunkSize < 0 || (chunkSize & (chunkSize - 1)) != 0){
            Debug.LogWarning("Chunk size is not a power of 8");
        }
    }
    
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
            Chunk newChunk = chunkBuilder.GetChunk(position, wallSettings.threshold);
            chunks.Add(position, newChunk);
            return newChunk;
        }
    }
}