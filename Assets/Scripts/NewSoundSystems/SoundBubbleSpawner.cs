using UnityEngine;
using UnityEngine.Pool;

/**
 * Not the most efficient pool, you could have a data driven system that would use less memory and processing
 * Also right now only used for hearing detection, but could easely be extendend to be used instead of particle system
 * in Ecolocator, all in one sulution with collisions and effects
 *                  -Jack
 * 
 * */


/// <summary>
/// This will also spawn bubbles as game objects instead of particles to get more controll
/// over the bubble, for collisions mostly.
/// </summary>
public class SoundBubbleSpawner : MonoBehaviour
{
    public SoundBubble prefab;
    ObjectPool<SoundBubble> pool;
    public float soundSpeed = 12; //could be nicer but all sounds grow at the same speed...
                                  //public float emitDelay = .3f;
                                  //private float emitTime;

    Transform bubbleParent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pool = new(OnCreateFunc, OnGetAction, OnReturnAction, OnDestroyAction, maxSize: 1000);
        if(TryGetComponent(out Echolocator echolocator))
        {
            soundSpeed = echolocator.soundSpeed;
            echolocator.OnEmit += EmitBubble;
        }
        bubbleParent = new GameObject("SoundBubbleParent").transform;
    }

    private SoundBubble OnCreateFunc()
    {
        
        var bubble = Instantiate(prefab, bubbleParent);
        bubble.Init(this); 
        return bubble;
    }

    private void OnGetAction(SoundBubble bubble)
    {
        //bubble.
    }

    private void OnReturnAction(SoundBubble bubble)
    {
        bubble.Disable();
    }

    private void OnDestroyAction(SoundBubble bubble)
    {
        Destroy(bubble.gameObject);
    }

    /// <summary>
    /// Emits a sount from this transforms location
    /// </summary>
    /// <param name="size">How big the bubble will grow</param>
    public void EmitBubble(float size = 10)
    {
        EmitBubble(transform.position, size);
        //float currentTime = Time.time;
        //if (currentTime > emitTime)
        //{
        //    emitTime = currentTime + emitDelay;
        //}
    }

    public void EmitBubble(Vector3 position, float size)
    {
        pool.Get(out SoundBubble bubble);
        bubble.Init(position, size);
    }

    public void Release(SoundBubble soundBubble)
    {
        pool.Release(soundBubble);
    }
}
