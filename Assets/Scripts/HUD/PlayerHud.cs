using System;
using UnityEngine;

public class PlayerHud : MonoBehaviour
{
     
    static event Action<GameObject> OnPlayer;
    public static GameObject playerObject;


    public static void SubscribeToPlayer(Action<GameObject> onPlayer)
    {
        OnPlayer += onPlayer;
        if (playerObject)
        {
            OnPlayer?.Invoke(playerObject);
        }
    }

    public static void UnsubscribeToPlayer(Action<GameObject> onPlayer)
    {
        OnPlayer -= onPlayer;
    }


    void Start()
    {
        playerObject = gameObject;
        OnPlayer?.Invoke(playerObject);
    }

    private void OnDestroy()
    {
        playerObject = null;
        OnPlayer?.Invoke(playerObject);

        OnPlayer = null;
    }

    //public void InvokeOnPlayer()
    //{
    //    OnPlayer?.Invoke(playerObject);
    //}



    //private void OnEnable()
    //{
    //    if (disableOnDisable)
    //    {
    //        OnPlayer?.Invoke(gameObject);
    //    }
    //}

    //private void OnDisable()
    //{
    //    if (disableOnDisable)
    //    {
    //        OnPlayer?.Invoke(null);
    //    }
    //}

}
