using UnityEngine;
using System.Linq;
using System;
using System.Numerics;

public class AudioLoudnessDetection : MonoBehaviour
{
    public int sampleWindow = 2048; // Number of samples to analyze
    public AudioClip microphoneAudioClip;

    private void Start()
    {
        MicrophoneToAudioClip();
    }

    public void MicrophoneToAudioClip()
    {
        string microphoneName = Microphone.devices[0];
        Debug.Log(microphoneName);

        microphoneAudioClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
        
    }

    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneAudioClip);
    }

    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;

        if (startPosition < 0)
        {
            return 0;
        }

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);

        float totalLoudness = 0;

        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness / sampleWindow;
    }

    // New method to detect pitch
    public float GetPitchFromMicrophone()
    {
        int position = Microphone.GetPosition(Microphone.devices[0]);
        return GetPitchFromAudioClip(position, microphoneAudioClip);
    }

    public float GetPitchFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;
        if (startPosition < 0) return 0;

        // Get audio data from clip
        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);

        // Convert to Complex array for FFT
        Complex[] complexData = new Complex[sampleWindow];
        for (int i = 0; i < waveData.Length; i++)
        {
            complexData[i] = new Complex(waveData[i], 0); // Imaginary part is 0
        }

        // Perform FFT on complexData
        FFT.FFTCompute(complexData);

        // Calculate the dominant frequency from FFT result
        float maxAmplitude = 0;
        int maxIndex = 0;
        float sampleRate = AudioSettings.outputSampleRate;

        // Threshold for minimum amplitude to consider for pitch detection -- we don care about the low anyway...?
        //float amplitudeThreshold = 0.01f; // Adjust as necessary --- expose this in snspector for tweaks??

        for (int i = 1; i <= complexData.Length / 2; i++)
        {
            float amplitude = (float)complexData[i].Magnitude;

            // Ignore low amplitude values (noise)
            //if (amplitude < amplitudeThreshold) continue;

            // Find the frequency bin with the highest amplitude
            if (amplitude > maxAmplitude)
            {
                maxAmplitude = amplitude;
                maxIndex = i;
            }
        }

        // Convert index to frequency only if a valid pitch was detected
        float dominantFrequency = 0;
        if (maxAmplitude > 0)
        {
            dominantFrequency = maxIndex * sampleRate / sampleWindow;


            //maybe clamp instead??
            // Check if the detected frequency is within a reasonable range (e.g., 20 Hz - 3000 Hz)
            //if (dominantFrequency < 20 || dominantFrequency > 3000)
            //{
            //    dominantFrequency = 0; // Reset if out of range
            //}

            dominantFrequency = Mathf.Clamp(dominantFrequency, 20, 3000);
        }

        // Log and return the detected pitch
        //Debug.Log("Detected Pitch: " + dominantFrequency + " Hz");
        return dominantFrequency;
    }



}

