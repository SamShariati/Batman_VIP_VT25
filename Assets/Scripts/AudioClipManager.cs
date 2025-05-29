using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipManager : MonoBehaviour
{

    [Header("Spider Reference")]
    public SpiderBTManager spider;

    [Header("Audio Settings")]
    public AudioSource audioSource;

    [Header("Transition Settings")]
    [Range(0.5f, 5f)]
    public float fadeTime = 2f;

    private int currentTrackIndex = 0;
    private int lastMusicState = 0;
    private bool isTransitioning = false;

    //LÄGG AUDIOCLIP VARIABLER HÄR
    [Header("AUDIOCLIPS")]
    [SerializeField] public AudioClip keyPickUpSound;
    [SerializeField] public AudioClip stonePickUpSound;
    [SerializeField] public AudioClip unlockChestSound;

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
            spider = FindObjectOfType<SpiderBTManager>();

        // Start with patrol music
        if (musicTracks.Count > 0 && musicTracks[0] != null)
        {
            audioSource.clip = musicTracks[0];
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("SpiderMusicManager: No patrol music assigned!");
        }
    }

    private void Update()
    {
        if (spider == null) return;

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
        if (spider.alertStateActivated)
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

}