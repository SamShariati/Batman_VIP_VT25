using UnityEngine;
using UnityEngine.Events;

public class InteractableButton : MonoBehaviour, IInteractable
{
    public string tooltip = "Press {0} button!";
    public string interactedTooltip = "Get F*cked";

    public string Tooltip => tooltip;
    public string InteractedTooltip => interactedTooltip;

    public bool CanInteract => enabled;

    public UnityEvent<Transform> OnPress;
    public void Interact(Transform interactor)
    {
        OnPress?.Invoke(interactor);
    }

    public void SpeculateInteract(Transform interactor)
    {
        //noop
    }
}