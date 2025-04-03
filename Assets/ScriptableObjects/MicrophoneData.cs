using UnityEngine;

[CreateAssetMenu(fileName = "MicrophoneData", menuName = "ScriptableObjects/MicrophoneData", order = 1)]
public class MicrophoneData : ScriptableObject
{
    public string microphone;
    public string[] avalibleMicrophones;
    
    
    private void OnValidate()
    {
        avalibleMicrophones = Microphone.devices;
    }
}
