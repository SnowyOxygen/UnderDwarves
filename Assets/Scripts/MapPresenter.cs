using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Handles viewing chunks in relation to the player's position
public class MapPresenter : MonoBehaviour
{
    private Map map;
    [SerializeField] private Transform player;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile wallTile;

    //Variables
    [SerializeField] private int viewDistance = 4;
    private Vector2Int playerChunkPos;

    // Chunks
    private Dictionary<Vector2Int, Chunk> visibleChunks = new Dictionary<Vector2Int, Chunk>();

    private void Start() {
        // Get map singleton
        map = Map.instance;

        playerChunkPos = Vector2Int.zero;
        UpdateChunks();
    }

    private void Update() {
        if(CheckPlayerPosition()) UpdateChunks();
    }

    // Return true if player chunk position has changed
    private bool CheckPlayerPosition(){
        Vector2 playerPos = player.position;

        Vector2Int chunkPos = map.WorldToChunkPos(playerPos);

        if(chunkPos != playerChunkPos){
            playerChunkPos = chunkPos;
            return true;
        }
        else return false;
    }

    // Check which chunks should be visible or hidden
    public void UpdateChunks(){
        Debug.Log($"Player Chunk is {playerChunkPos}");

        List<Vector2Int> positions = new List<Vector2Int>();

        int startX = playerChunkPos.x - viewDistance;
        int endX = playerChunkPos.x + viewDistance;
        int startY = playerChunkPos.y - viewDistance;
        int endY = playerChunkPos.y + viewDistance;

        for(int x = startX; x <= endX; x++){
            for(int y = startY; y <= endY; y++){
                Vector2Int chunk = new Vector2Int(x, y);
                positions.Add(chunk);          
            }
        }

        // Get chunks from map
        Chunk[] chunks = map.GetChunks(positions);

        // Hide chunks
        HideChunks(chunks.ToList());

        // Show chunks
        ViewChunks(chunks);
    }

    // Show chunks inside of the viewer distance
    private void ViewChunks(Chunk[] chunks){
        foreach(Chunk chunk in chunks){
            // Debug.Log($"Chunk position is {chunk.position}");
            if(!visibleChunks.ContainsValue(chunk)){
                ViewChunk(chunk);
            }
        }
    }

    private void ViewChunk(Chunk chunk){
        int[,] walls = chunk.walls;

        // View chunks in a grid around the player
        for(int x = 0 ; x < map.chunkSize; x++){
            for(int y = 0; y < map.chunkSize; y++){
                Vector3Int position = new Vector3Int(
                    x + map.chunkSize * chunk.position.x,
                    y + map.chunkSize * chunk.position.y,
                    0
                );
                if(walls[x, y] == 1){
                    // Create wall
                    tilemap.SetTile(position, wallTile);
                }
            }
        }

        visibleChunks.Add(chunk.position, chunk);
    }

    // Hide chunks outside of the viewer distance
    private void HideChunks(List<Chunk> safe){
        List<Vector2Int> toRemove = new List<Vector2Int>();

        foreach(KeyValuePair<Vector2Int, Chunk> valuePair in visibleChunks){
            Chunk chunk = valuePair.Value;
            Vector2Int position = valuePair.Key;

            if(!safe.Contains(chunk)){
                HideChunk(chunk);
                toRemove.Add(chunk.position);
            }
        }

        Debug.Log($"{toRemove.Count} chunks removed");
        foreach(Vector2Int e in toRemove) visibleChunks.Remove(e);
    }

    private void HideChunk(Chunk chunk){
        int[,] walls = chunk.walls;

        for(int x = 0 ; x < map.chunkSize; x++){
            for(int y = 0; y < map.chunkSize; y++){
                Vector3Int position = new Vector3Int(
                    x + map.chunkSize * chunk.position.x,
                    y + map.chunkSize * chunk.position.y,
                    0
                );
                tilemap.SetTile(position, null);
            }
        }
    }
}
