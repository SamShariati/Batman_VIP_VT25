using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugIcon : MonoBehaviour
{

    public RectTransform rect;
    public TMP_Text text;
    public Image image;


    void Awake()
    {
        rect = GetComponent<RectTransform>();
        text = GetComponentInChildren<TMP_Text>();
        image = GetComponentInChildren<Image>();

    }

    public void SetColor(Color color)
    {
        text.color = color;
        image.color = color;
    }

    public void Hide(bool hide)
    {
        gameObject.SetActive(!hide);
    }

    public void SetPosition(Vector3 pos)
    {
        rect.position = pos;
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }

    void Update()
    {
        
    }
}
