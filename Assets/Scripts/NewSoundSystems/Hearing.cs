using System.Collections;
using UnityEngine;
/**
 * Used for testing purposes but file does include the ISoundListener interface...
 * 
 */
public class Hearing : MonoBehaviour, ISoundListener
{
    Color color;
    MeshRenderer rend;
    Vector3 eulers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        color = rend.material.color;
        eulers = Random.onUnitSphere * 360;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
      //  Debug.Log("Hit by particle!");
        StopAllCoroutines();
        StartCoroutine(DoEffect());
    }

    public void HearSound(Transform soundSource, Vector3 soundOrigin)
    {
      //  Debug.Log("Hearing a sound!");
        StopAllCoroutines();
        StartCoroutine(DoEffect());
    }

    IEnumerator DoEffect()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material.color = Color.red;
        
        float t = 1;
        
        while (t > 0)
        {
            renderer.material.color = Color.Lerp(color, Color.red, t);
            t-= Time.deltaTime*2;
            transform.Rotate(t * Time.deltaTime * eulers);
            yield return null;  
        }
    }
}

public interface ISoundListener
{
    /// <summary>
    /// Since sound have travel time Transform soundSource could have moved or even been destroyed by the time this method is called... probably not destroyed tho
    /// </summary>
    /// <param name="soundSource"></param>
    /// <param name="soundOrigin">where the sound was spawned</param>
    public void HearSound(Transform soundSource, Vector3 soundOrigin);
}