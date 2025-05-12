using UnityEngine;

public class UpdatePlayerManager : MonoBehaviour
{
    Echolocator eco;
    PlayerController playerController;
    void Start()
    {
        eco = GetComponentInChildren<Echolocator>();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        PlayerManager.Instance.isCurrentlyMakingSound = eco.LoudnessNormalized > eco.LoudnessThreshold || playerController.MovementNoiseLevel > playerController.soundRadiusCrouch;
        PlayerManager.Instance.normalizedSoundLevel = Mathf.Max(eco.LoudnessNormalized, playerController.MovementNoiseLevel, 1);
        PlayerManager.Instance.soundPosition = transform.position;
       // playerController.
    }
}
