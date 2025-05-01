using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public event Action<bool> CanInteract;

    [Tooltip("Will auto reference child with name LookDir")][SerializeField] private Transform lookDir;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range = 2;

    [SerializeField] PlayerInput playerInput;
    InputAction action;
    string key = "[error]";
    bool interactLastFrame;
    IInteractable interactable;

    // Start is called before the first frame update
    void Start()
    {
        if (!lookDir) lookDir = Camera.main.transform;
        Debug.Assert(lookDir, "Interactor - lookDir not assigned!");

        action = playerInput.actions.FindAction("Interact");
        key = "[" + action.bindings.First().ToDisplayString() + "]";


    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(lookDir.position, lookDir.forward, out RaycastHit hit, range, layerMask))
        {
            IInteractable item = hit.transform.GetComponentInParent<IInteractable>();
            //Debug.Log("Hit:" + hit.transform.gameObject.name);

            if (item != null && item.CanInteract)
            {
                if (interactable != item)
                {
                    interactable = item;
                    item.SpeculateInteract(transform);
                }
                CanInteract?.Invoke(true);
                TooltipUtil.Show(string.Format(item.Tooltip, key));
                if (action.triggered)
                {
                    item.Interact(transform);
                    TooltipUtil.Show(string.Format(item.InteractedTooltip, key), 1.5f, 1);
                }
            }
            else //we hit something but cannot interact with it
            {
                if (interactable != null) CanInteract?.Invoke(false); //no need to sen each frame
                interactable = null;
            }
            //Debug.DrawLine(transform.position, hit.point, Color.red);

        }
        else //we hit nothing and thus cannot interact
        {
            if(interactable != null) CanInteract?.Invoke(false); //no need to sen each frame we miss
            interactable = null;
        }

    }
}