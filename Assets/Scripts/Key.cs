using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    public string keyColour = "Red";

    public string Tooltip => "Press {0} to pick up " + keyColour + " key";

    public string InteractedTooltip => "Picked up " + keyColour + " key!";

    public bool CanInteract => true;

    public void Interact(Transform interactor)
    {
        if(interactor.TryGetComponent(out TempKeyInventory inventory))
        {
            if (inventory.PickUpKey(keyColour))
            {
                gameObject.SetActive(false);
                inventory.PickUpAnim();
            }
            else
            {
                //what?
            }
        }
    }

    public void SpeculateInteract(Transform interactor)
    {
        if (interactor.TryGetComponent(out TempKeyInventory inventory))
        {
            if (inventory.HasKey(keyColour))
            {
                //we can tooltip that player already has this key
            }
        }
    }

    void Start()
    {
        
    }

}
