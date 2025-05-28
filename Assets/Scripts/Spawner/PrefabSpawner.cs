using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{

    public GameObject prefab;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        transform.ForEveryChild((x) => { Instantiate(prefab, x.position, Quaternion.identity); });
    }

    
}
