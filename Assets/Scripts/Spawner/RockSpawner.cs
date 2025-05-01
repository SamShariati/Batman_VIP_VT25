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

    public override void Init()
    {
        //incase we need to load somthing before
    }


    //rooms are the rooms, each room contains a list of spawn points
    //select spawn points and spawn stuff at them THEN REMOVE the spawn point for the next spawner
    //so all spawns passed on are valid!
    public override List<Room> Spawn(List<Room> rooms)
    {
        if (stoneTestPrefab == null || stoneCoverage <= 0)
        {
            return rooms;
        }

        for (int i = 0; i < rooms.Count; i++)
        {
            for (int j = 0; j < rooms[i].spawnPoints.Count; j++)
            {
                Transform t = rooms[i].spawnPoints[j];
                if (stoneCoverage > Random.value) {

                    int s = Random.Range(0, stoneTestPrefab.Length);
                    Instantiate(stoneTestPrefab[s], t.position, Quaternion.identity);
                    rooms[i].spawnPoints.RemoveAt(j--); //remember to remove the spawn point if we spawn here so we dont double spawn
                }
            }
        }


        return rooms;   
    }

    public override void FinalTouches() { 
        //after all spawners have run do something extra?
    
    }

}
