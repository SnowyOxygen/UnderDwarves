using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Walker
{
    public static Vector2Int[] directions = new Vector2Int[4]{
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };

    public static Vector2Int[] GetPositionsFromOrigin(Vector2Int origin, int amount){
        List<Vector2Int> positions = new List<Vector2Int>();
        positions.Add(origin);

        for(int i = 0; i < amount; i++){
            List<Vector2Int> choices = directions.ToList();

            Vector2Int? next = null;

            while(next == null){
                if(choices.Count == 0){
                    i = amount;
                    positions.Clear();
                    positions.Add(origin);
                    break;
                }

                Vector2Int choice = choices[Random.Range(0, choices.Count - 1)];
                choices.Remove(choice);

                Vector2Int candidate = choice + positions.Last();
                if(positions.Contains(candidate)){
                    next = candidate;
                }
            }

            if(next == null) continue;
            
            positions.Add((Vector2Int)next);
        }

        return positions.ToArray();
    }
}
