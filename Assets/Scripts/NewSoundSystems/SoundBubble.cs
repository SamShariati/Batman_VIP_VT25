using UnityEngine;


/**
 * Grows at a fixed rate determined bu spawner, has a sphere collider trigger for callbacks
 * that will call ISoundListener if possible on target
 * -Jack
 * 
 */

public class SoundBubble : MonoBehaviour
{

    private float size;
    //private float currentSize;
    private SoundBubbleSpawner spawner;
    internal void Disable()
    {
        gameObject.SetActive(false);
    }

    internal void Init(SoundBubbleSpawner pool)
    {
        this.spawner = pool;
    }

    internal void Init(Vector3 position, float size)
    {
        transform.position = position;
        this.size = size;
        transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
    }

    void Update()
    {
        transform.localScale += spawner.soundSpeed * Time.deltaTime * Vector3.one;
        if (transform.localScale.x >= size)
        {
            spawner.Release(this);
        }
        
    }

    /*
     * Does only work if other has a rigidbody... if not we re-think, maybe Physics.OverlapSphere, but that will retrigger each frame, so consider that...
     */
    private void OnTriggerEnter(Collider other)
    {
        //find out if enemy
        //pass data about player to enemy
        //pool.gameObject or use short hand...
        if(other.TryGetComponent(out ISoundListener listener)){
            listener.HearSound(spawner.transform, transform.position);
        }
    }
}
