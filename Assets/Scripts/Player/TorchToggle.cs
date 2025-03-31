using UnityEngine;

public class TorchToggle : MonoBehaviour
{
    [SerializeField] private GameObject torchObject;
    [SerializeField] private KeyCode toggleKey = KeyCode.T;

    void Update()
    {
        if(Input.GetKeyUp(toggleKey))
        {
            torchObject.SetActive(!torchObject.activeSelf);
        }
    }
}
