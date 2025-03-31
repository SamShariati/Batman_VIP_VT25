using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMaker : MonoBehaviour
{
    [SerializeField] private AudioSource source = null;
    [SerializeField] private float soundRange = 25f;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform spiderTransform;

    private float pulseDistance;
    private EcholocationController echolocationController;

    private void Start()
    {
        if (echolocationController == null)
        {
            echolocationController = FindObjectOfType<EcholocationController>();
            if (echolocationController == null)
            {
                Debug.LogError("EcholocationController not found in the scene!");
            }
        }
    }

    private void Update()
    {
        if (echolocationController == null || playerTransform == null || spiderTransform == null)
        {
            Debug.LogWarning("Missing references in SoundMaker!");
            return;
        }

        pulseDistance = echolocationController.GetPulseDistance();

        soundRange = Mathf.Min(pulseDistance, echolocationController.GetMaxDistance()); // Cap by the max distance of the pulse

        float distanceToPlayer = Vector3.Distance(spiderTransform.position, playerTransform.position);

        if (pulseDistance > 0 && Mathf.Abs(pulseDistance - distanceToPlayer) < 0.5f) // Adjust tolerance if needed
        {
            Debug.Log("Pulse detected in SoundMaker, emitting sound...");
            EmitSound(); // Trigger sound for the AI
        }

        // Debugging to monitor pulseDistance and distanceToPlayer
        //Debug.Log($"Pulse Distance: {pulseDistance}, Distance to Player: {distanceToPlayer}");



        //if (echolocationController == null || playerTransform == null || spiderTransform == null)
        //    return;

        //// Get the current pulse distance
        //pulseDistance = echolocationController.GetPulseDistance();

        //// Calculate the distance between the spider and the player
        //float distanceToPlayer = Vector3.Distance(spiderTransform.position, playerTransform.position);

        //// If the pulse distance matches the distance to the player, trigger a sound
        //if (Mathf.Abs(pulseDistance - distanceToPlayer) < 0.5f) // Adjust tolerance as needed
        //{
        //    TriggerSound();
        //}








        //// Check if the Space key is pressed
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    // Check if the Space key is pressed
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        // Check if the AudioSource is assigned and not playing
        //        if (source != null && !source.isPlaying)
        //        {
        //            Debug.Log("AudioSource assigned and ready to play sound"); // Log for AudioSource status

        //            var sound = new Sound(transform.position, soundRange);
        //            sound.soundType = Sound.SoundType.Danger;

        //            print($"Sound: with pos {sound.pos} and range {sound.range} created!");

        //            Sounds.MakeSound(sound);

        //            // Play the sound after all other operations
        //            source.Play();
        //        }
        //        else if (source == null)
        //        {
        //            Debug.LogWarning("AudioSource is not assigned in SoundMaker script.");
        //        }
        //    }
        //}
    }
    private void TriggerSound()
    {
        // Check if the AudioSource is assigned and not playing
        if (source != null && !source.isPlaying)
        {
            Debug.Log("AudioSource assigned and ready to play sound");

            var sound = new Sound(transform.position, soundRange);
            sound.soundType = Sound.SoundType.Danger;

            print($"Sound: with pos {sound.pos} and range {sound.range} created!");

            Sounds.MakeSound(sound);

            // Play the sound after all other operations
            source.Play();
        }
        else if (source == null)
        {
            Debug.LogWarning("AudioSource is not assigned in SoundMaker script.");
        }
    }
    private void EmitSound()
    {
        if (source != null && !source.isPlaying)
        {
            Debug.Log("SoundMaker: Emitting sound...");

            var sound = new Sound(transform.position, soundRange);
            sound.soundType = Sound.SoundType.Danger;

            print($"Sound: with pos {sound.pos} and range {sound.range} created!");
            Sounds.MakeSound(sound);

            source.Play(); // Play the sound
        }
        else if (source == null)
        {
            Debug.LogWarning("AudioSource is not assigned in SoundMaker script.");
        }
    }
}
