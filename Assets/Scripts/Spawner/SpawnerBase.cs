using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnerBase : MonoBehaviour
{

    [HideInInspector]public SpawnChests spawnChests;

    public abstract void Init();
    public abstract List<Room> Spawn(List<Room> rooms);

    public virtual void FinalTouches() { }
}
