using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class UpdatePlayerManager : MonoBehaviour
{
    Echolocator eco;
    PlayerController playerController;
    [SerializeField] float afterTalkTimer = 0.25f; 
    float afterTalkTime = 0; 
    bool talking = false;
    float level = 0;    
    void Start()
    {
        eco = GetComponentInChildren<Echolocator>();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (eco.DirectLoudnessNormalized > eco.LoudnessThreshold)
        {
            afterTalkTime = Time.time;
            level = 0.25f * Mathf.Max(level, eco.DirectLoudnessNormalized);
            talking = true;
        }
        if(afterTalkTime + afterTalkTimer < Time.time)
        {
            talking = false;
            level = 0;
        }

        PlayerManager.Instance.isCurrentlyMakingSound = talking || playerController.MovementNoiseLevel > playerController.soundRadiusCrouch;
        PlayerManager.Instance.normalizedSoundLevel = Mathf.Max(level, playerController.MovementNoiseLevel);
        PlayerManager.Instance.soundPosition = transform.position;
       // playerController.
    }
}
