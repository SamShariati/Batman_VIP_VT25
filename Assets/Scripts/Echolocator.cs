using System;
using UnityEngine;

public class Echolocator : MonoBehaviour
{
    public float soundSpeed = 5;
    public float minPulseDelay = 0.05f;
    public float maxPulseDelay = 0.5f;
    private float pulseDelay = 0.1f;
    private float pulseTime = 0.1f;
    public float loudnessThreshold = 0.1f;
    public float loudnessBoost = 1000;
    public float maxRange = 100f;
    public AudioLoudnessDetection audioLoudnessDetection;
    public bool useParticleSystem;
    public ParticleSystem system;

    public event Action<float> OnEmit;

    void Start()
    {
        system = GetComponent<ParticleSystem>();

        Debug.Assert(audioLoudnessDetection, "Echolocator - Assign lodness system in inspector!!");
    }

    // Update is called once per frame
    void Update()
    {
        if(pulseTime + pulseDelay > Time.time) return; //dont do anything if we on cooldown

        float loudness = Mathf.Clamp01(audioLoudnessDetection.GetLoudnessFromMicrophone() * loudnessBoost);

        

        if (loudness > loudnessThreshold)
        {
            pulseTime = Time.time;
            float pitch = audioLoudnessDetection.GetPitchFromMicrophone();

            pulseDelay = Mathf.Lerp(minPulseDelay, maxPulseDelay, Mathf.InverseLerp(400, 20, pitch)); //expose min and max pitch in inspector

            //pulseDelay = Mathf.Clamp(delay, minPulseDelay, 1);

            //Debug.Log("pitch = " + pitch + " pulseDelay = " + pulseDelay);
            PulseOnce(loudness);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            pulseTime = Time.time;
            pulseDelay = minPulseDelay;
            PulseOnce(1);
        }
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
