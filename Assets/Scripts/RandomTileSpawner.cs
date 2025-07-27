using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class RandomTileSpawner : MonoBehaviour
{
    private Tilemap targetTilemap;

    [Header("Prefab to spawn")]
    public GameObject prefabToSpawn;

    [Header("How many to spawn")]
    public int numberToSpawn = 5;

    [Header("Spawn on Start?")]
    public bool spawnOnStart = true;


    private List<Vector3Int> availableTiles = new List<Vector3Int>();

    void Awake()
    {
        targetTilemap = GetComponent<Tilemap>();
        if (targetTilemap == null)
            Debug.LogError("RandomTileSpawner: No Tilemap found on this GameObject!");
    }

    void Start()
    {
        if (spawnOnStart)
            SpawnPrefabs();
    }

    public void SpawnPrefabs()
    {
        if (targetTilemap == null || prefabToSpawn == null || numberToSpawn <= 0)
            return;

        availableTiles.Clear();
        foreach (var pos in targetTilemap.cellBounds.allPositionsWithin)
        {
            if (targetTilemap.HasTile(pos))
                availableTiles.Add(pos);
        }

        int spawnCount = Mathf.Min(numberToSpawn, availableTiles.Count);
        var usedIndices = new HashSet<int>();
        for (int i = 0; i < spawnCount; i++)
        {
            int idx;
            do {
                idx = Random.Range(0, availableTiles.Count);
            } while (usedIndices.Contains(idx));
            usedIndices.Add(idx);
            Vector3Int cellPos = availableTiles[idx];
            Vector3 worldPos = targetTilemap.GetCellCenterWorld(cellPos);
            Instantiate(prefabToSpawn, worldPos, Quaternion.identity);
        }
    }
}
