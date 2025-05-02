using UnityEngine;

public class LightToggle : MonoBehaviour
{

    public Color lightColor = Color.white;
    public Color darkColor = Color.black;

    public bool notDark = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Color col = notDark ? lightColor : darkColor;
        RenderSettings.ambientLight = col;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            notDark = !notDark;
            Color col = notDark ? lightColor : darkColor;
            RenderSettings.ambientLight = col;
        }
    }
}
