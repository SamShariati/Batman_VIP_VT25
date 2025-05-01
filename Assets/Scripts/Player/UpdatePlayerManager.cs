using UnityEngine;

public class UpdatePlayerManager : MonoBehaviour
{
    Echolocator eco;
    void Start()
    {
        eco = GetComponentInChildren<Echolocator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerManager.Instance.isCurrentlyMakingSound = eco.LoudnessNormalized > eco.LoudnessThreshold;
        PlayerManager.Instance.normalizedSoundLevel = eco.LoudnessNormalized;
        PlayerManager.Instance.soundPosition = transform.position;
    }
}
