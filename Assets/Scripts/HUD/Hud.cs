using System;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{

    GameObject playerObject;
    public event Action<GameObject> OnPlayerObject;

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
        
    }
}
