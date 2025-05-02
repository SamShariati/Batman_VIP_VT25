using System;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{

    GameObject playerObject;
    public event Action<GameObject> OnPlayerObject;
    public GameObject[] debugHUD;
    public bool showDebug;

    void Start()
    {
        if(TryGetComponent(out Image border))
        {
            border.enabled = false;
        }

       
        foreach (var item in debugHUD)
        {
            item.SetActive(showDebug);
        }
        

        PlayerHud.SubscribeToPlayer(OnPlayer);
    }

    private void OnDestroy()
    {
        PlayerHud.UnsubscribeToPlayer(OnPlayer);
    }

    private void OnPlayer(GameObject obj)
    {
        if (!obj)
        {
            gameObject.SetActive(false);
            OnPlayerObject(null);
            return;
        }
        gameObject.SetActive(true);
        if (obj != playerObject)
        {
            playerObject = obj;
            OnPlayerObject(playerObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            showDebug = !showDebug;
            foreach (var item in debugHUD)
            {
                item.SetActive(showDebug);
            }
        }
    }
}
