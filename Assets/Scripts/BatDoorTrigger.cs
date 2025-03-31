using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatDoorTrigger : MonoBehaviour
{
    [SerializeField] Animator doorAnimator;
    MeshRenderer meshRenderer;

    // Start is called before the first frame update
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bat")
        {
            doorAnimator.SetTrigger("OpenDoor");
        }
    }
    
}
