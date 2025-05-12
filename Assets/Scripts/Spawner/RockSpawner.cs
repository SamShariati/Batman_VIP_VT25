using System.Collections.Generic;
using UnityEngine;

/**
 * spawns a rock randomly, this does nog gaurantee a rock in each room so if you want that more sophisticated algo is needed
 * but just rewite this...
 * 
 * Also needs to add random rotation (and scale?)
 * 
 * Stones are not allowed wo cover more that on spawn point, if it does this script are responsible for removing affected spawn point
 */


public class RockSpawner : SpawnerBase
{
    public GameObject[] stoneTestPrefab;
    [Range(0, 1)] public float stoneCoverage = .5f;

    [Header("Scale Range")]
    public float minScale = 0.5f;
    public float maxScale = 1.5f;

    [Header("Per Room Density")]
    public int minStonesPerRoom = 4;
    public int maxStonesPerRoom = 6;

    [Tooltip("Blocking layers")]
    public LayerMask collisionMask;

    public float baseCollisionRadius = 0.5f;
    [Tooltip("Retry attemts")]
    public int maxAttemptsPerStone = 5;

    [Header("Ground snap")]
    public float raycastHeight = 2f;
    public float raycastDepth = 10f;

    public override void Init()
    {
        //incase we need to load somthing before
    }
    //rooms are the rooms, each room contains a list of spawn points
    //select spawn points and spawn stuff at them THEN REMOVE the spawn point for the next spawner
    //so all spawns passed on are valid!
    public override List<Room> Spawn(List<Room> rooms)
    {
        foreach (var room in rooms)
        {
            SpawnRoom(room);
        }

        return rooms;
    }
    //Room spawning logic
    void SpawnRoom(Room room)
    {
        var points = new List<Transform>(room.spawnPoints);
        int target = Mathf.Min(
            Random.Range(minStonesPerRoom, maxStonesPerRoom + 1),
            points.Count
        );

        int spawned = 0;
        while (spawned < target && points.Count > 0)
        {
            // pick and remove a random point
            int idx = Random.Range(0, points.Count);
            var t = points[idx];
            points.RemoveAt(idx);

            if (TrySpawnAt(t.position))
                spawned++;
        }

        // hand back the unused points
        room.spawnPoints = points;
    }

    //Spawning stones at worldPos up to max attemts
    bool TrySpawnAt(Vector3 worldPos)
    {
        for (int attempt = 0; attempt < maxAttemptsPerStone; attempt++)
        {
            Vector3 spawnPos = GetGroundPosition(worldPos);
            float scaleFactor = Random.Range(minScale, maxScale);
            float radius = baseCollisionRadius * scaleFactor;

            if (!Physics.CheckSphere(spawnPos, radius, collisionMask))
            {
                var prefab = stoneTestPrefab[Random.Range(0, stoneTestPrefab.Length)];
                var instance = Instantiate(prefab, spawnPos, Quaternion.identity);
                instance.transform.localScale = prefab.transform.localScale * scaleFactor;
                return true;
            }
        }
        return false;
    }

    //Raycast to find ground
    Vector3 GetGroundPosition(Vector3 pos)
    {
        var origin = pos + Vector3.up * raycastHeight;
        if (Physics.Raycast(origin, Vector3.down, out var hit, raycastDepth))
            return hit.point;
        return pos;
    }

    public override void FinalTouches() { 
        //after all spawners have run do something extra?
    
    }

}
