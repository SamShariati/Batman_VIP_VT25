using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class FrequencyVisualizer : MonoBehaviour
{
    public MicrophoneData microphoneData;
    public RawImage imageTemplate;
    
    List<RawImage> bars;
    AudioClip microphoneAudioClip;

    void Start()
    {

        if (!microphoneData)
        {
            Debug.LogError("Assign mic data!");
            enabled = false;
            return;
        }
        bars = new();
        for (int i = 0; i < microphoneData.sampleWindow/2; i++)
        {
            var bar = Instantiate(imageTemplate, transform);
            bar.gameObject.SetActive(true);
            bars.Add(bar);
        }   
        MicrophoneToAudioClip();
    }

    public void MicrophoneToAudioClip()
    {
        microphoneAudioClip = Microphone.Start(microphoneData.microphone, true, 20, AudioSettings.outputSampleRate);
    }

    // Update is called once per frame
    void Update()
    {
        int clipPosition = Microphone.GetPosition(microphoneData.microphone);
        int sampleWindow = microphoneData.sampleWindow;
        int startPosition = clipPosition - sampleWindow;
        if (startPosition < 0)
        {
            //Debug.Log("Mic 0 pos..."); //for now to check and delete this later
            return;
        }

        // Get audio data from clip
        float[] waveData = new float[sampleWindow];
        microphoneAudioClip.GetData(waveData, startPosition);

        // Convert to Complex array for FFT
        Complex[] complexData = new Complex[sampleWindow];
        for (int i = 0; i < waveData.Length; i++)
        {
            complexData[i] = new Complex(waveData[i], 0); // Imaginary part is 0
        }

        // Perform FFT on complexData
        FFT.FFTCompute(complexData);

        // Calculate the dominant frequency from FFT result
        float sampleRate = AudioSettings.outputSampleRate; //for maybe 



        for (int i = 1; i <= complexData.Length / 2; i++)
        {
            float amplitude = (float)complexData[i].Magnitude;

            bars[i - 1].rectTransform.sizeDelta = new UnityEngine.Vector2(10, amplitude);
            //bars[i - 1].
        }
    }
}
