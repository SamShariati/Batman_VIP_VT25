using UnityEngine;

public class SpiderSFXManager : MonoBehaviour
{
    public static SpiderSFXManager instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    // Play a specific audio clip
    public void PlaySFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        // Assign the audioClip and volume
        audioSource.clip = audioClip;
        audioSource.volume = volume;

        // Play sound
        audioSource.Play();

        // Destroy clip
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    // Play a random audio clip from an array
    public void PlayRandomSFXClip(AudioClip[] audioClips, Transform spawnTransform, float volume)
    {
        // null check
        if (audioClips == null || audioClips.Length == 0)
        {
            Debug.LogWarning("AudioClip array is null or empty.");
            return;
        }

        int rand = Random.Range(0, audioClips.Length);

        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        // Assign the audioClip aand volume
        audioSource.clip = audioClips[rand];
        audioSource.volume = volume;

        audioSource.Play();

        // Destroy clip after play
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    // Play a specific audio clip by index from an array
    public void PlaySFXFromArray(AudioClip[] audioClips, int index, Transform spawnTransform, float volume)
    {
        // null check
        if (audioClips == null || index < 0 || index >= audioClips.Length)
        {
            Debug.LogWarning("Invalid index or audioClip array is null.");
            return;
        }

        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClips[index];
        audioSource.volume = volume;

        audioSource.Play();

        // Destroy clip after play
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
}
