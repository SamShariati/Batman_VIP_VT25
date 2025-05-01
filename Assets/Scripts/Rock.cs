using UnityEngine;

public class Rock : MonoBehaviour, IInteractable
{

    public string Tooltip => "Press {0} to pick up rock";

    public string InteractedTooltip => "Picked up rock";

    public bool CanInteract => true;

    public void Interact(Transform interactor)
    {
        if (interactor.TryGetComponent(out KeyRockHandler inventory))
        {
            if (inventory.PickUpRock())
            {
                Destroy(gameObject);
            }
        }
    }

    public void SpeculateInteract(Transform interactor)
    {
        if (interactor.TryGetComponent(out KeyRockHandler inventory))
        {
            
        }
    }
}
