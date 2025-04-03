using UnityEngine;

public class EcholocationController : MonoBehaviour
{
    [Header("Echolocation material")]
    [SerializeField] private Material _echolocationMaterial;
    [SerializeField] private Color _pulseColor = Color.white;
    [SerializeField] private float _pulseSpeed = 5.0f;
    [SerializeField] private float _pulseWidth = 6.0f;
     private float _maxDistance;
    [SerializeField] private float _edgeStrength = 1.0f;
    [SerializeField] private float _constantLightIntensity = 0.12f;
    [SerializeField] private float _constantLightMaxDistance = 3.5f;
    [SerializeField] private Camera _cameraToEmitPulseFrom;
    [SerializeField] private Camera _cameraToEmitConstantLightFrom;

    [Header("Audio Settings")]
    [SerializeField] private AudioLoudnessDetection _audioDetector;
    [SerializeField] private float loudnessSensitivity = 1.0f;
    [SerializeField] private float loudnessThreshold = 0.1f; // Threshold to trigger the pulse
    [SerializeField] private float _echolocationCooldown = 5;

    private float pulseDistance = 500000.0f;
    private bool pulseTriggered = false;

    private float _echolocationTimer;

    void Update()
    {
        // Get loudness from microphone
        float loudness = _audioDetector.GetLoudnessFromMicrophone() * loudnessSensitivity;

        // Trigger the pulse when loudness exceeds the threshold
        if ((loudness > loudnessThreshold && !pulseTriggered && Input.GetKey(KeyCode.V)) || (!pulseTriggered && Input.GetKeyDown(KeyCode.P)))
        {
            pulseDistance = 0.0f; // Reset pulse distance
            pulseTriggered = true; // Prevent retriggering immediately
            _echolocationTimer = _echolocationCooldown;
            _maxDistance = loudness * 20;
            _maxDistance = 20;
            Debug.Log("Pulse triggered by sound!");
        }
     

        if(pulseTriggered)
        {
            _echolocationTimer -= Time.deltaTime;

            if(_echolocationTimer <= 0.0f)
            {
                pulseTriggered = false;
            }
        }

        // Increase the pulse distance over time
        pulseDistance += Time.deltaTime * _pulseSpeed;

        // Set shader parameters
        _echolocationMaterial.SetColor("_PulseColor", _pulseColor);
        _echolocationMaterial.SetFloat("_PulseDistance", pulseDistance);
        _echolocationMaterial.SetFloat("_PulseWidth", _pulseWidth);
        _echolocationMaterial.SetFloat("_MaxDistance", _maxDistance);
        _echolocationMaterial.SetFloat("_EdgeStrength", _edgeStrength);
        _echolocationMaterial.SetFloat("_ConstantLightIntensity", _constantLightIntensity);
        _echolocationMaterial.SetFloat("_ConstantLightMaxDistance", _constantLightMaxDistance);

        // Set the pulse camera position dynamically
        Vector3 pulseCameraPosition = _cameraToEmitPulseFrom.transform.position;
        _echolocationMaterial.SetVector("_CameraPosition", pulseCameraPosition);

        // Set the constant light camera position dynamically
        Vector3 constantLightCameraPosition = _cameraToEmitConstantLightFrom.transform.position;
        _echolocationMaterial.SetVector("_ConstantLightCameraPosition", constantLightCameraPosition);

        // Debugging loudness
        Debug.Log($"Loudness: {loudness}, Pulse Distance: {pulseDistance}");
    }
    public float GetPulseDistance()
    {
        return pulseDistance;
    }
    public float GetMaxDistance()
    {
        return _maxDistance;
    }
}



