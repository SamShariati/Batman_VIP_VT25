using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolNodeScript : MonoBehaviour
{
    MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }
}
