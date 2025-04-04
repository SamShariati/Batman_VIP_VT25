using UnityEngine;

[CreateAssetMenu(fileName = "MicrophoneData", menuName = "ScriptableObjects/MicrophoneData", order = 1)]
public class MicrophoneData : ScriptableObject
{
    [Tooltip("Amount of samples for frequency analysis")]
    public int sampleWindow = 2048;
    [Tooltip("How much to boost the sounds, they are tiny tiny numbers otherwise...")]
    public float micBoost = 300;
    [Tooltip("Normalized - cuts off sound below this")]
    public float threshold = 0.1f;
    [Tooltip("Will use this mic if it exists or be set to first avalible mic on start")]
    public string microphone;
    public string[] avalibleMicrophones;

    private void OnEnable()
    {
        ValidateMic();
    }

    [MakeButton]
    public void ValidateMic()
    {
        avalibleMicrophones = Microphone.devices;
        bool micExists = false;
        for (int i = 0; i < avalibleMicrophones.Length; i++)
        {
            if (avalibleMicrophones[i].Equals(microphone))
            {
                micExists = true;
                break;
            }
        }
        if (!micExists)
        {
            microphone = avalibleMicrophones[0];
        }
    }

    private void OnValidate()
    {
        //avalibleMicrophones = Microphone.devices;
    }
}
