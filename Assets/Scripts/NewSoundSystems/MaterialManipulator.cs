using System.Collections;
using UnityEngine;

public class MaterialManipulator : MonoBehaviour, ISoundListener
{
    public MeshRenderer meshRenderer;
    Material material;
    public float maxTime = 1;
    public float maxRadius = 4;
    public float speed = 1;
    public Color color = Color.blue;
    WaitForSeconds wait;
    public bool ready = true;
    void Start()
    {
        if(!meshRenderer)meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;
        //material.
        wait = new WaitForSeconds(.3f);
        meshRenderer.enabled = false;
        //material.SetFloat("_Speed", speed);
        //material.SetColor("_Color", color);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HearSound(Transform soundSource, Vector3 soundOrigin)
    {
        if(ready)
            StartCoroutine(FadeEffect(maxTime));

        if(TryGetComponent(out DynamicNoiseMaker noiseMaker)){
            noiseMaker.Emit(maxRadius, false);
        }
    }

    private IEnumerator FadeEffect(float time)
    {
        meshRenderer.enabled = true;
        ready = false;
        material.SetFloat("_Speed", speed);
        material.SetColor("_Color", color);
        yield return new WaitForSeconds(time);
        //float time = maxTime;
        //while (time > 0)
        //{
        //  time -= Time.deltaTime;
        //}
        meshRenderer.enabled = false;
        ready = true;
    }
}
