using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to easily apply the bat material to the bat map.
/// </summary>
public class BatMapMaterial : MonoBehaviour
{
    public GameObject batMap;
    public Material echolocationMaterial;

    void Start()
    {
        if (batMap == null || echolocationMaterial == null)
        {
            Debug.LogError("ASSIGN BAT MAP AND ECHO MATERIAL");
            return;
        }

        MeshRenderer[] renderers = batMap.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material = echolocationMaterial;
        }

        Debug.Log("APPLIED ALL MATERIAL TO BAT MAPE");
    }
}