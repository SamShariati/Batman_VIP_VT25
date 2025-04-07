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
    public float width = 10;
    public float boost = 10;
    [Range(0,1)]
    public float smoothing = .6f;
    //public int micFrequency = 1000;
    float[] lastFrameData;
    public AnimationCurve filter;
    void Start()
    {

        if (!microphoneData)
        {
            Debug.LogError("Assign mic data!");
            enabled = false;
            return;
        }
        bars = new();
        for (int i = 0; i < microphoneData.sampleWindow / 2 ; i++)
        {
            var bar = Instantiate(imageTemplate, transform);
            bar.gameObject.SetActive(true);
            bars.Add(bar);
            float freq = i * microphoneData.sampleRate / microphoneData.sampleWindow;
            //Debug.Log("Bin_" + i + " = " + freq + "Hz");
        }   

        width = GetComponent<RectTransform>().sizeDelta.x / bars.Count; //auto fit tinto parent panel
        lastFrameData = new float[bars.Count];

        MicrophoneToAudioClip();
    }

    public void MicrophoneToAudioClip()
    {
        microphoneAudioClip = Microphone.Start(microphoneData.microphone, true, 20, microphoneData.sampleRate);
        Debug.Log("Microphone frequency = " + microphoneAudioClip.frequency);
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
        //float sampleRate = AudioSettings.outputSampleRate; //for maybe 



        for (int i = 1; i <= complexData.Length / 2; i++)
        {
            float amplitude = (float)complexData[i].Magnitude * boost;

            float value = Mathf.Lerp(amplitude, lastFrameData[i - 1], smoothing);

            value = filter.Evaluate(Mathf.InverseLerp(1, complexData.Length / 2, i)) * value;

            value = Mathf.Clamp(value, 1.0f, 100); //add max like size of container

            bars[i-1].rectTransform.sizeDelta = new UnityEngine.Vector2(width, value);

            lastFrameData[i - 1] = value;

        }

    }
}
