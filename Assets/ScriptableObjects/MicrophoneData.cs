using System.Numerics;
using UnityEngine;

[CreateAssetMenu(fileName = "MicrophoneData", menuName = "ScriptableObjects/MicrophoneData", order = 1)]
public class MicrophoneData : ScriptableObject
{
    [Tooltip("Amount of samples for loudness check")]
    public int sampleLoudnessWindow = 64;
    [Tooltip("Normalized - cuts off sound below this")]
    public float threshold = 0.1f;
    [Tooltip("Clamps sound louder than this")]
    public float maxLoudness = 50.0f;
    [Tooltip("Max Amplitude")]
    public float maxAmplitude = 23000;
    [Tooltip("Frequency of the microphone")]
    public int sampleRate = 2024;
    [Tooltip("Amount of samples for frequency analysis")]
    public int sampleWindow = 512;
    [Tooltip("How much to boost the sounds, they are tiny tiny numbers otherwise...")]
    public float micBoost = 300;
    [Tooltip("Will use this mic if it exists or be set to first avalible mic on start")]
    public string microphone;
    public string[] avalibleMicrophones;


    //[Range(0, 1)]
    //public float smoothing = .6f;
    ////public int micFrequency = 1000;
    //float[] lastFrameData;
    //public bool useFilter = true;
    //public AnimationCurve filter;
    //public float freqBoost = 300;

    private void OnEnable()
    {
        
        ValidateMic();

        if (Application.isPlaying)
        {
           // microphoneAudioClip = Microphone.Start(microphone, true, 20, sampleRate);

        }
        //Debug.Log("Channels = " + microphoneAudioClip.channels);
    }

    

    [MakeButton]
    public void ValidateMic()
    {
        avalibleMicrophones = Microphone.devices;
        if(avalibleMicrophones == null || avalibleMicrophones.Length == 0)
        {
            return;
        }


        bool micExists = false;
        for (int i = 0; i < avalibleMicrophones.Length; i++)
        {
            if (avalibleMicrophones[i].Equals(microphone))
            {
                micExists = true;
                break;
            }
        }
        if (!micExists && avalibleMicrophones.Length > 0)
        {
            microphone = avalibleMicrophones[0];
        }

        //lastFrameData = new float[sampleWindow / 2];
    }

    private void OnValidate()
    {
        //avalibleMicrophones = Microphone.devices;
    }

    public int GetPosition()
    {
        return string.IsNullOrEmpty(microphone) ? 0 : Microphone.GetPosition(microphone);
    }

}
