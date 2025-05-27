using UnityEngine;

public class LoadSettingsOnStart : MonoBehaviour
{
    public MicrophoneData microphoneData;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (microphoneData) 
        {
            microphoneData.Load(); 
        }
        else
        {
            Debug.LogWarning("Mic Settings not loaded");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
