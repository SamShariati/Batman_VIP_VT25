using TMPro;
using UnityEngine;

public class PlayerManagerHUD : MonoBehaviour
{

    public TMP_Text isMakingSound;
    public TMP_Text soundLevel;
    public TMP_Text other;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isMakingSound) isMakingSound.text = "IsMakingSound = " + (PlayerManager.Instance.isCurrentlyMakingSound ? "Yes":"No");
        if(soundLevel) soundLevel.text = "SoundLevel = " + PlayerManager.Instance.normalizedSoundLevel.ToString("n2");
    }
}
