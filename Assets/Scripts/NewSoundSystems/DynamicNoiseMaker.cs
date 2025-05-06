using UnityEngine;

public class DynamicNoiseMaker : MonoBehaviour
{
    SoundBubbleSpawner SoundBubbleSpawner;
    Rigidbody Rigidbody;
    public float maxSoundRange = 15;
    public float minImpact = .5f;
    private float lastEmitTime;
    public float maxEmitTime = .5f;
    public float rotationMultiplier = .5f;
    public float velocityMultiplier = .5f;
    public AudioClip[] clips;

    void Awake()
    {
        SoundBubbleSpawner = FindAnyObjectByType<SoundBubbleSpawner>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Emit(float range, bool collision = true)
    {
        if(lastEmitTime + maxEmitTime < Time.time)
        {
            lastEmitTime = Time.time;
            SoundBubbleSpawner.EmitBubble(transform.position, Mathf.Min(range, maxSoundRange), collision);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float I = collision.impulse.sqrMagnitude;
        if (I > minImpact && SoundBubbleSpawner)
        {
            Emit(Mathf.Min(I, maxSoundRange));
            AudioFX.Instance.PlayClip(clips, transform.position);
        }
        Rigidbody.angularVelocity *= rotationMultiplier;
        Rigidbody.linearVelocity *= velocityMultiplier;
    }
}
