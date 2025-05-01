using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{

    //feel free to add what ever...
    public bool isCurrentlyMakingSound = false;
    public float normalizedSoundLevel = 0;
    public Vector3 soundPosition = Vector3.zero;
}
