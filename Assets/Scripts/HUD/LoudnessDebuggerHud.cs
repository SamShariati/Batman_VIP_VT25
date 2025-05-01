using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoudnessDebuggerHud : HudBase
{
    Echolocator echolocator;


    public Image loudnessImg;
    public Image loudnessThresholdImg;
    public Image loudnessFloaterImg;
    public TMP_Text loudnessValueText;
    public TMP_Text loudnessFloaterText;
    private float loudnessFloater;
    public float loudnessFloaterSpeed = 0.6f;
    public float loudnessFloaterDelay = 0.6f;
    private float loudnessFloaterTimer;

    public override void OnNoPlayer()
    {
        
    }

    public override void OnPlayerObject(GameObject obj)
    {
        echolocator = obj.GetComponentInChildren<Echolocator>();
        enabled = echolocator;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!echolocator)
        {
            enabled = false;
            return;
        }

        float dt = Time.deltaTime;

        float loudness = echolocator.DirectLoudnessNormalized;
        loudnessFloater = echolocator.LoudnessNormalized;



        //if(loudnessFloaterTimer <= 0)
        //{
        //    loudnessFloater -= dt * loudnessFloaterSpeed;
        //    loudnessFloaterTimer = 0;
        //}

        //loudnessFloaterTimer -= dt;

        //if (loudness > loudnessFloater)
        //{
        //    loudnessFloater = loudness;
        //    loudnessFloaterTimer = loudnessFloaterDelay;
        //}


        loudnessImg.fillAmount = echolocator.DirectLoudnessNormalized;
        loudnessValueText.text = loudness.ToString("n2");


        loudnessFloaterImg.fillAmount = echolocator.LoudnessNormalized;
        loudnessFloaterText.text = loudnessFloater.ToString("n2");


        float t = echolocator.microphoneData.threshold;
        loudnessThresholdImg.rectTransform.anchoredPosition = new Vector2(5, 10 + t * 480);
    }
}
