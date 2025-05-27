using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipManager : MonoBehaviour
{
    //LÄGG AUDIOCLIP VARIABLER HÄR
    [SerializeField] public AudioClip placeHolderSound;

    private void Awake()
    {
        GameObject.DontDestroyOnLoad(this);
    }
}