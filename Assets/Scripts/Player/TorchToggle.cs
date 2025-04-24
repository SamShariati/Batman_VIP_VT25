using UnityEngine;

public class TorchToggle : MonoBehaviour
{
    //[SerializeField] private GameObject torchObject;
    [SerializeField] private Animator torchAnimator;
    [SerializeField] private KeyCode toggleKey = KeyCode.T;
    bool toggle = false;

    private void Start()
    {
        toggle = true;
        torchAnimator.SetBool("TorchUp", toggle);
    }

    void Update()
    {
        if(Input.GetKeyDown(toggleKey))
        {
            //torchObject.SetActive(!torchObject.activeSelf);

            toggle = !toggle; 
            torchAnimator.SetBool("TorchUp", toggle);
        }
    }
}
