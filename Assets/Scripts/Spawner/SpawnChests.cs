using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Room
{

    public int id;
    public string name;
    public List<Transform> spawnPoints;
}



public class SpawnChests : MonoBehaviour
{
    public Transform addRoomParent;
    public Chest chestPrefab;
    public Chest finalChest; // this might be achest in the end...
    public List<SpawnerBase> spawners = new();
    public Key keyPrefab;
    public string[] keys;
    public List<Room> rooms = new();
    private List<Room> rooms2 = new();
    public List<Chest> chests = new();
    public Key key;
    public event Action<List<Room>> OnChestSpawned;

    void Start()
    {
        ArrayUtil.Shuffle(keys);

        for (int i = 1; i < keys.Length; i++)
        {
            string keyToOpen = keys[i-1];
            string keyToSpawn = keys[i];
            int roomId = Random.Range(0, rooms.Count);
            int spawnId = Random.Range(0, rooms[roomId].spawnPoints.Count);
            Transform transform = rooms[roomId].spawnPoints[spawnId];
            //rooms[roomId].id = roomId;
            rooms[roomId].spawnPoints.RemoveAt(spawnId);
            rooms[roomId].name = keyToOpen;
            rooms2.Add(rooms[roomId]);

            rooms.RemoveAt(roomId); //remove the room, we have spawned a chest in it
            SpawnChest(transform, keyToOpen, keyToSpawn);
        }
        int lastSpawnId = Random.Range(0, rooms[0].spawnPoints.Count);
        Transform keyTransform = rooms[0].spawnPoints[lastSpawnId];
        SpawnKey(keyTransform.position + Vector3.up * .5f, keys[0]);

        rooms[0].spawnPoints.RemoveAt(lastSpawnId);
        rooms2.Add(rooms[0]);

        if(finalChest)finalChest.requiredKey = keys[^1]; //last key for ther final cherts

        foreach (var spawner in spawners)
        {
            spawner.spawnChests = this;
            spawner.Init();
            spawner.Spawn(rooms2);
        }

        foreach (var spawner in spawners)
        {
            spawner.FinalTouches();
        }
    }

    private void SpawnChest(Transform transform, string keyToOpen, string keyToSpawn)
    {
        //do a better job on the spawn position
        
        Chest chest = Instantiate(chestPrefab, transform.position, Quaternion.identity);
        chest.requiredKey = keyToOpen;
        chest.requireKey = true;
        chests.Add(chest);
        SpawnKey(chest.KeyPosition, keyToSpawn); //spawn the ky in the chest
    }

    private void SpawnKey(Vector3 position, string key)
    {
        //do a better job on the spawn position
        Key keyInstance = Instantiate(keyPrefab, position, Quaternion.identity);
        keyInstance.keyColour = key;
        keyInstance.gameObject.name = key + "Key";
        this.key =  keyInstance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnValidate()
    {
        if (addRoomParent)
        {
            if(addRoomParent.childCount > 0)
            {

                Transform[] children = new Transform[addRoomParent.childCount];
                for (int i = 0; i < addRoomParent.childCount; i++)
                {
                    children[i] = addRoomParent.GetChild(i);
                }

                rooms.Add(new Room { 
                    id = rooms.Count, 
                    name = addRoomParent.name,
                    spawnPoints = new(children)
                });
                Debug.Log("Added a room: " + addRoomParent.name);
            }
            
            
            addRoomParent = null;
        }
    }
}
