using UnityEngine;

public class DynamicNoiseMaker : MonoBehaviour
{
    SoundBubbleSpawner SoundBubbleSpawner;
    Rigidbody Rigidbody;
    public float maxSoundRange = 15;
    public float minImpact = .5f;
    void Start()
    {
        SoundBubbleSpawner = FindAnyObjectByType<SoundBubbleSpawner>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        float I = collision.impulse.sqrMagnitude;
        if (I > minImpact && SoundBubbleSpawner)
        {
            SoundBubbleSpawner.EmitBubble(transform.position, Mathf.Max(I, maxSoundRange));
        }
    }
}
