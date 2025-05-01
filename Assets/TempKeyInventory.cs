using System.Collections.Generic;
using UnityEngine;

public class TempKeyInventory : MonoBehaviour
{

    HashSet<string> keys = new();

    void Start()
    {
        
    }

    public bool HasKey(string key)
    {
        return keys.Contains(key);
    }

    public bool PickUpKey(string key)
    {
        return keys.Add(key);
    }
}
