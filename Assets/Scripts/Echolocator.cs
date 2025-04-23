using System;
using System.Numerics;
using System.Text;
using UnityEngine;

public class Echolocator : MonoBehaviour
{
    [Header("Sound and Loudness Settings")]
    [Tooltip("How fast a pulse bubble grows in m/s")]
    public float soundSpeed = 5;
    [Tooltip("Minimum time between pulses in seconds")] public float minPulseDelay = 0.05f;
    [Tooltip("Maximum pulse reset time in seconds")] public float maxPulseDelay = 0.5f;
    private float pulseDelay = 0.1f;
    private float pulseTime = 0.1f;
    //public float loudnessThreshold = 0.1f;
    //[Tooltip("Some mics are low (should be primarily tweaked in MicrophoneData for settings access)")]
    //public float loudnessBoost = 1000;
    [Tooltip("How far can a pulse reach in meters")]
    public float maxRange = 100f;
    private float loudnessFloater;
    [Tooltip("How fast the loudness decreases")]public float loudnessFloaterSpeed = 0.6f;
    [Tooltip("Delay befor loudness starts decreasing")] public float loudnessFloaterDelay = 0.6f;
    private float loudnessFloaterTimer;
    float loudness;


    [Header("Setup")]
    public MicrophoneData microphoneData;
   // public AudioLoudnessDetection audioLoudnessDetection;
    public bool useParticleSystem;
    public ParticleSystem system;

    AudioClip microphoneAudioClip;
    [Header("Frequency Settings??")]
    [Range(0, 1)]
    public float smoothing = .6f;
    float[] lastFrameData;
    public bool useFilter = true;
    public AnimationCurve filter;


    public event Action<float> OnEmit;

    public float Loudness { get; private set; }
    public float LoudnessNormalized { get; private set; }
    public float DirectLoudness { get; private set; }
    public float DirectLoudnessNormalized { get; private set; }
    public float Pitch { get; private set; }
    public float MaxLoudness { get; private set; }


    public float[] FrequencyData => lastFrameData;
    //public float LoudnessThreshold => loudnessThreshold;


    public float minTime = 0.05f;
    public float maxTime = 0.5f;
    private float timer;
    private float time;

    void Awake()
    {
        system = GetComponent<ParticleSystem>();

        //Debug.Assert(audioLoudnessDetection, "Echolocator - Assign loudness system in inspector!!");
        Debug.Assert(microphoneData, "Echolocator - Assign microphoneData in inspector!!");

        lastFrameData = new float[microphoneData.sampleRate / 2];
        MicrophoneToAudioClip();
    }

    public void MicrophoneToAudioClip()
    {
        microphoneAudioClip = Microphone.Start(microphoneData.microphone, true, 20, microphoneData.sampleRate);
        Debug.Assert(microphoneAudioClip, "microphoneAudioClip creation failed");
        Debug.Log("Microphone: " + microphoneData.microphone + " with sampling frequency: " + microphoneAudioClip.frequency);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMic();

        DirectLoudness = loudness;
        DirectLoudnessNormalized = Mathf.InverseLerp(0, microphoneData.maxLoudness, loudness);

        float dt = Time.deltaTime;
        if (loudnessFloaterTimer <= 0)
        {
            loudnessFloater -= dt * loudnessFloaterSpeed * microphoneData.maxLoudness;
            loudnessFloaterTimer = 0;
        }

        loudnessFloaterTimer -= dt;

        if (loudness > loudnessFloater)
        {
            loudnessFloater = loudness;
            loudnessFloaterTimer = loudnessFloaterDelay;
        }
        Loudness = loudnessFloater;
        LoudnessNormalized = Mathf.InverseLerp(0, microphoneData.maxLoudness, loudnessFloater);

        if (pulseTime + pulseDelay > Time.time) return; //dont do anything if we on cooldown

        //Loudness = Mathf.Clamp01(audioLoudnessDetection.GetLoudnessFromMicrophone() * loudnessBoost);
        //Loudness = GetLoudness();
        //Loudness = audioLoudnessDetection.GetLoudnessFromMicrophone() * loudnessBoost;

        //Loudness = GetLoudness

        if (LoudnessNormalized > microphoneData.threshold)
        {
            pulseTime = Time.time;
            //float pitch = audioLoudnessDetection.GetPitchFromMicrophone();

            //remap!!
            //pulseDelay = Mathf.Lerp(minPulseDelay, maxPulseDelay, Mathf.InverseLerp(400, 20, pitch)); //expose min and max pitch in inspector

            //pulseDelay = Mathf.Clamp(delay, minPulseDelay, 1);

            //Debug.Log("pitch = " + pitch + " pulseDelay = " + pulseDelay);
            PulseOnce(Loudness);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            pulseTime = Time.time;
            pulseDelay = minPulseDelay;
            PulseOnce(1);
        }
    }

    private void UpdateMic()
    {
        int clipPosition = Microphone.GetPosition(microphoneData.microphone);
        int sampleWindow = microphoneData.sampleWindow;
        int startPosition = clipPosition - sampleWindow;
        if (startPosition < 0)
        {
            return;
        }

        // Get audio data from clip
        float[] waveData = new float[sampleWindow* microphoneAudioClip.channels];
        Debug.Assert(microphoneAudioClip, "microphoneAudioClip error");
        Debug.Assert(waveData.Length == sampleWindow, "error");
        //Debug.Log(startPosition + " startPosition channels: " + microphoneAudioClip.channels);
        microphoneAudioClip.GetData(waveData, startPosition);

        float loudness = 0;

        // Convert to Complex array for FFT also find loudness
        Complex[] complexData = new Complex[sampleWindow];
        for (int i = 0; i < waveData.Length; i++)
        {

            loudness += Mathf.Abs(waveData[i])*microphoneData.micBoost;

            complexData[i] = new Complex(waveData[i], 0); // Imaginary part is 0
        }
        loudness = Mathf.Min(loudness / sampleWindow, microphoneData.maxLoudness);

        //loudness = Mathf.InverseLerp(0, microphoneData.maxLoudness, loudness / sampleWindow); //normalize


        //DirectLoudness = loudness;
        this.loudness = loudness;

        // Perform FFT on complexData
        FFT.FFTCompute(complexData);

       // Debug.Log("..." + loudness);
        //we could find largest peak here too for dominant frequency... TODO
        //StringBuilder sb = new();
        for (int i = 1; i <= complexData.Length / 2; i++)
        {
            float amplitude = (float)complexData[i].Magnitude;

            float value = Mathf.Lerp(amplitude, lastFrameData[i - 1], smoothing);
            if (useFilter)
            {
                value = filter.Evaluate(Mathf.InverseLerp(1, complexData.Length / 2, i)) * value;
            }

            //value = Mathf.Clamp01(value); //add max like size of container


            lastFrameData[i - 1] = value;
           // sb.Append(value).Append(' ');
        }
        //Debug.Log(sb.ToString());
    }

    public float[] GetFrequencyData()
    {
        return lastFrameData;
    }

    private float GetLoudness(int clipPosition, int sampleWindow)
    {
        int startPosition = clipPosition - sampleWindow;
        if (startPosition < 0)
        {
            return 0;
        }
        float[] waveData = new float[sampleWindow];
        microphoneAudioClip.GetData(waveData, startPosition);
        float loudness = 0;
        for (int i = 0; i < waveData.Length; i++)
        {
            loudness += waveData[i];
        }
        return loudness / sampleWindow;
    }


    public void PulseOnce(float loudness)
    {
        float range = maxRange * loudness;
        if (useParticleSystem) { 
            var emitParams = new ParticleSystem.EmitParams();
            float time = range / soundSpeed; // v = s/t  physics!
            emitParams.startSize = range;
            emitParams.startLifetime = time;
            system.Emit(emitParams, 1);
        }

        OnEmit?.Invoke(range);
    }
}
