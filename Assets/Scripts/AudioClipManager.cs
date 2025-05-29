using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipManager : MonoBehaviour
{
    [Header("Spider Reference (Optional)")]
    public SpiderBTManager spider;

    [Header("Audio Settings")]
    public AudioSource audioSource;

    [Header("Transition Settings")]
    [Range(0.5f, 5f)]
    public float fadeTime = 2f;

    [Header("Music Settings")]
    [Tooltip("Music to play when no spider is present (e.g., menu scenes)")]
    public int defaultMusicTrack = 0;

    [Tooltip("Whether to play music automatically when no spider is found")]
    public bool playDefaultMusicWhenNoSpider = true;

    private int currentTrackIndex = 0;
    private int lastMusicState = 0;
    private bool isTransitioning = false;
    private bool hasSpider = false;

    //LÄGG AUDIOCLIP VARIABLER HÄR
    [Header("AUDIOCLIPS")]
    [SerializeField] public AudioClip keyPickUpSound;
    [SerializeField] public AudioClip stonePickUpSound;
    [SerializeField] public AudioClip unlockChestSound;

    [Header("PLAYER SOUNDS")]
    [SerializeField] public AudioClip[] stepSounds;

    [Header("SPIDER SOUNDS")]
    [SerializeField] public AudioClip spiderKillSound;
    [SerializeField] public AudioClip spiderFleeSound;
    [SerializeField] public AudioClip[] spiderScreamSounds;
    [SerializeField] public AudioClip[] spiderScreechSounds;
    [SerializeField] public AudioClip spiderWalkingSound;
    [SerializeField] public AudioClip spiderRunningSound;

    [Tooltip("0: Patrol Music, 1: Alert Music")]
    public List<AudioClip> musicTracks = new List<AudioClip>(3);

    private void Awake()
    {
        //GameObject.DontDestroyOnLoad(this);
    }

    private void Start()
    {
        // Find spider if not assigned
        if (spider == null)
            spider = FindFirstObjectByType<SpiderBTManager>();

        hasSpider = spider != null;

        // Determine which music to start with
        int startingTrack = hasSpider ? 0 : defaultMusicTrack;

        // Start music if we have tracks available
        if (musicTracks.Count > startingTrack && musicTracks[startingTrack] != null)
        {
            if (hasSpider || playDefaultMusicWhenNoSpider)
            {
                currentTrackIndex = startingTrack;
                audioSource.clip = musicTracks[startingTrack];
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            Debug.LogWarning($"AudioClipManager: No music track at index {startingTrack} assigned!");
        }

        // Log spider status for debugging
        if (!hasSpider)
        {
            Debug.Log("AudioClipManager: No spider found in scene. Using default music behavior.");
        }
    }

    private void Update()
    {
        // Only do dynamic music switching if we have a spider
        if (!hasSpider) return;

        // Check spider's state directly
        int currentMusicState = GetMusicStateFromSpider();

        if (currentMusicState != lastMusicState)
        {
            lastMusicState = currentMusicState;
            ChangeToTrack(currentMusicState);
        }
    }

    private int GetMusicStateFromSpider()
    {
        // This should only be called when hasSpider is true
        if (spider != null && spider.alertStateActivated)
            return 1; // Alert music

        return 0; // Patrol music (default)
    }

    private void ChangeToTrack(int newTrackIndex)
    {
        // Validate track exists and is different
        if (newTrackIndex < musicTracks.Count &&
            musicTracks[newTrackIndex] != null &&
            newTrackIndex != currentTrackIndex &&
            !isTransitioning)
        {
            StartCoroutine(TransitionToMusic(newTrackIndex));
        }
    }

    // Public method to manually change music (useful for menu/cutscenes)
    public void SetMusicTrack(int trackIndex)
    {
        if (trackIndex >= 0 && trackIndex < musicTracks.Count && musicTracks[trackIndex] != null)
        {
            ChangeToTrack(trackIndex);
        }
        else
        {
            Debug.LogWarning($"AudioClipManager: Invalid track index {trackIndex}");
        }
    }

    // Public method to stop music
    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            StartCoroutine(FadeOutAndStop());
        }
    }

    private IEnumerator TransitionToMusic(int newTrackIndex)
    {
        isTransitioning = true;

        // Fade out current music
        yield return StartCoroutine(FadeOut());

        // Switch to new track
        currentTrackIndex = newTrackIndex;
        audioSource.clip = musicTracks[currentTrackIndex];

        // Fade in new music
        yield return StartCoroutine(FadeIn());

        isTransitioning = false;
    }

    private IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop();
    }

    private IEnumerator FadeIn()
    {
        audioSource.Play();
        audioSource.volume = 0;

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, 1, t / fadeTime);
            yield return null;
        }

        audioSource.volume = 1;
    }

    private IEnumerator FadeOutAndStop()
    {
        isTransitioning = true;
        yield return StartCoroutine(FadeOut());
        isTransitioning = false;
    }
}