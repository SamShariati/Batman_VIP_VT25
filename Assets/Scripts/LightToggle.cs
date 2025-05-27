using UnityEngine;

public class LightToggle : MonoBehaviour
{

    public Color lightColor = Color.white;
    public Color darkColor = Color.black;
    Light dirLight;

    public bool notDark = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Color col = notDark ? lightColor : darkColor;
        RenderSettings.ambientLight = col;
        Light[] lights = FindObjectsByType<Light>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var light in lights)
        {
            if(light.type == LightType.Directional)
            {
                dirLight = light;
                break;
            }
        }
        if (dirLight && !notDark)
        {
            dirLight.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            notDark = !notDark;
            Color col = notDark ? lightColor : darkColor;
            if (dirLight)
            {
                dirLight.enabled = notDark;
            }
            RenderSettings.ambientLight = col;
        }
    }
}
