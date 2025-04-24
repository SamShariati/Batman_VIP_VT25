using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

[Serializable]
public class Room
{

    public int id;
    public string name;
    public Transform[] spawnPoints;
}

public class SpawnChests : MonoBehaviour
{
    public Transform addRoomParent;
    public Chest chestPrefab;
    public Key keyPrefab;
    public string[] keys;
    public List<Room> rooms = new();

    void Start()
    {
        for (int i = 1; i < keys.Length; i++)
        {
            string keyToOpen = keys[i-1];
            string keyToSpawn = keys[i];
            int roomId = Random.Range(0, rooms.Count);
            int spawnId = Random.Range(0, rooms[roomId].spawnPoints.Length);
            Transform transform = rooms[roomId].spawnPoints[spawnId];
            rooms[roomId].name = keyToOpen;
            rooms.RemoveAt(roomId); //remove the room, we have spawned a chest in it
            SpawnChest(transform, keyToOpen, keyToSpawn);
        }
        int lastSpawnId = Random.Range(0, rooms[0].spawnPoints.Length);
        Transform keyTransform = rooms[0].spawnPoints[lastSpawnId];
        SpawnKey(keyTransform.position, keys[0]);

    }

    private void SpawnChest(Transform transform, string keyToOpen, string keyToSpawn)
    {
        //do a better job on the spawn position
        
        Chest chest = Instantiate(chestPrefab, transform.position, Quaternion.identity);
        chest.requiredKey = keyToOpen;
        chest.requireKey = true;
        SpawnKey(chest.KeyPosition, keyToSpawn); //spawn the ky in the chest
    }

    private void SpawnKey(Vector3 position, string key)
    {
        //do a better job on the spawn position
        Key keyInstance = Instantiate(keyPrefab, position, Quaternion.identity);
        keyInstance.keyColour = key;
        keyInstance.gameObject.name = key + "Key";
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
                    spawnPoints = children
                });
                Debug.Log("Added a room: " + addRoomParent.name);
            }
            
            
            addRoomParent = null;
        }
    }
}
