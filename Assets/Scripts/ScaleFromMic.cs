using UnityEngine;


public class ScaleFromMic : MonoBehaviour
{
    public AudioSource source;
    public Vector3 minScale;
    public Vector3 maxScale;

    public AudioLoudnessDetection detector;

    public float loudnessSensibility = 1;
    public float threshold = 0.01f;

    // Add variables for pitch detection
    public float targetPitch = 440f; // Target pitch (A4)
    public float pitchTolerance = 5f; // Allowable deviation from the target pitch

    private void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;

        if (loudness < threshold)
        {
            loudness = 0;
        }

        transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);
        Debug.Log("Loudness: " + loudness);
        float pitch = detector.GetPitchFromMicrophone();
    }
}

