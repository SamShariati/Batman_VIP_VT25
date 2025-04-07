using System;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{

    GameObject playerObject;

    void Start()
    {
        if(TryGetComponent(out Image border))
        {
            border.enabled = false;
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
            return;
        }
        gameObject.SetActive(true);
        if (obj != playerObject)
        {
            playerObject = obj;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
