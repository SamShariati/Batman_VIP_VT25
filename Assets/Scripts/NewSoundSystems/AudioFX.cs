using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AudioFX : PersistentSingleton<AudioFX>
{

    [SerializeField] AudioSource template;
    ObjectPool<AudioSource> pool;
    //public int maxClipsPlaying = 20;
    //public int clipsPlaying;

    //a pool is overkill we can have a list and find least time left and replace (then keep amount of sounds at a decent level)
    //to many will lead to cutoffs and som odd stuff
    //List<AudioSource> list = new();

    protected override void Awake()
    {
        base.Awake();
        if (!template) template = new GameObject("soundClipObject").GetOrAdd<AudioSource>(); //not optimal as it spawns it in the active scene, also not in the pool
        template.gameObject.hideFlags = HideFlags.HideAndDontSave;
        template.playOnAwake = false;
        template.loop = false;
        template.spatialBlend = 1.0f; //full 3d
        template.priority = 200; //lower prority than default
        //template.maxDistance = 100;
        //template.transform.parent = transform;
        //list.Add(template);
        //for (int i = 0; i < maxClipsPlaying-1; i++)
        //{
        //    var a = Instantiate(template, transform);
        //    list.Add(a);
        //}

        pool = new(() => Instantiate(template, transform) /*, (x) => { x.gameObject.SetActive(true); }, (x) => { x.gameObject.SetActive(false); }*/);
    }



    public void PlayClip(AudioClip clip, Vector3 position, float volume = 1.0f)
    {
        if(!clip) return;

        //if (clipsPlaying >= maxClipsPlaying) {
        //    float minTimeLeft = float.MaxValue;
        //    int leastTimeId = 0;
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        float timeLeft = list[i].clip.length - list[i].time;
        //        if(timeLeft < minTimeLeft)
        //        {
        //            minTimeLeft = timeLeft;
        //            leastTimeId = 0;
        //        }
        //    }
        //    AudioSource audioSource = list[leastTimeId];
        //}

        AudioSource audioSource = pool.Get();
        audioSource.transform.position = position;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        //clipsPlaying++;
        StartCoroutine(Release(audioSource, clip.length));
    }

    public void PlayClip(AudioClip[] clips, Vector3 position, float volume = 1.0f)
    {
        if(clips != null && clips.Length > 0)
        {
            PlayClip(clips[Random.Range(0, clips.Length)], position, volume);
        }
    }


    private IEnumerator Release(AudioSource source, float t)
    {
        yield return new WaitForSeconds(t);
        //clipsPlaying--;
        pool.Release(source);
    }
}
