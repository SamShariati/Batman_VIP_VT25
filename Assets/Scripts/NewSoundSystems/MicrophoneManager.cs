using System.Numerics;
using UnityEngine;

public class MicrophoneManager : MonoBehaviour
{
    
    [SerializeField] MicrophoneData microphoneData;
    AudioClip microphoneAudioClip;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    public void StartMic()
    {
        microphoneAudioClip = Microphone.Start(microphoneData.microphone, true, 20, microphoneData.sampleRate);
    }
    
    void Update()
    {
        
    }



    public float GetLoudness()
    {
        return GetLoudnessFromAudioClip(microphoneData.GetPosition(), microphoneAudioClip);
    }

    public float GetLoudnessNormalized()
    {
        float loadness = GetLoudness();

        return Mathf.InverseLerp(0, microphoneData.maxAmplitude, loadness);
    }

    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {

        int startPosition = clipPosition - microphoneData.sampleLoudnessWindow;

        if (startPosition < 0)
        {
            Debug.Log("...");
            return 0;
        }

        float[] waveData = new float[microphoneData.sampleLoudnessWindow];
        clip.GetData(waveData, startPosition);

        float totalLoudness = 0;

        for (int i = 0; i < microphoneData.sampleLoudnessWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }
        Debug.Log("GetLoudnessFromAudioClip " + totalLoudness);

        return totalLoudness / microphoneData.sampleLoudnessWindow * microphoneData.micBoost;
    }

    public void GetFrequencyData(float[] data)
    {
        int clipPosition = microphoneData.GetPosition();
        int startPosition = clipPosition - microphoneData.sampleWindow;
        if (startPosition < 0)
        {
            //Debug.Log("Mic 0 pos..."); //for now to check and delete this later
            return;
        }



        //Debug.Log(startPosition + "startPosition");
        // Get audio data from clip
        float[] waveData = new float[microphoneData.sampleWindow];
        microphoneAudioClip.GetData(waveData, startPosition);

        // Convert to Complex array for FFT
        Complex[] complexData = new Complex[microphoneData.sampleWindow];
        for (int i = 0; i < waveData.Length; i++)
        {
            complexData[i] = new Complex(waveData[i], 0); // Imaginary part is 0
        }

        // Perform FFT on complexData
        FFT.FFTCompute(complexData);

        // Calculate the dominant frequency from FFT result
        //float sampleRate = AudioSettings.outputSampleRate; //for maybe 



        //for (int i = 1; i <= complexData.Length / 2; i++)
        //{
        //    float amplitude = (float)complexData[i].Magnitude * microphoneData.freqBoost;

        //    float value = Mathf.Lerp(amplitude, microphoneData.lastFrameData[i - 1], microphoneData.smoothing);
        //    if (microphoneData.useFilter)
        //    {
        //        value = microphoneData.filter.Evaluate(Mathf.InverseLerp(1, complexData.Length / 2, i)) * value;
        //    }



        //    microphoneData.lastFrameData[i - 1] = value;
        //    data[i - 1] = value;
        //}

    }
}
