using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Chest : MonoBehaviour, IInteractable
{
    public Transform lid;
    public float lidSpeed = 5;
    public Animator animator; // if we want to animate later?
    public string tooltip = "Press {0} to open!";
    public string interactedTooltip = "Chest opened";
    public bool requireKey = false;
    public string requiredKey = "Red";

    float target;
    float current;
    bool open = false;

    public string Tooltip => tooltip;
    public string InteractedTooltip => interactedTooltip;

    public bool CanInteract { get; set; }
    public Vector3 KeyPosition => transform.position + Vector3.up;

    public event Action OnOpened;

    public void Interact(Transform interactor)
    {

        if (open) { 
            CloseChest(); // we can always close a chest (no key)
        } 
        else if (!requireKey)
        {
            OpenChest();
        }
        else if (interactor.TryGetComponent(out KeyRockHandler inventory))
        {
            if (!open && inventory.HasKey(requiredKey))
            {
                OpenChest();
                inventory.UseKey();
            }
            else 
            {
                interactedTooltip = "Lol... " + requiredKey + " key needed!";
            }
        }
    }

    public void SpeculateInteract(Transform interactor)
    {

        if (open)
        {
            tooltip = "Press {0} to close " + requiredKey + " chest";
            return;
        }
        
        if (!requireKey)
        {
            return;
        }
        if (interactor.TryGetComponent(out KeyRockHandler inventory))
        {
            if (inventory.HasKey(requiredKey))
            {



                tooltip = "Press {0} to open " + requiredKey + " chest";
            }
            else
            {
                tooltip = "Requires " + requiredKey + " key to open";
            }
        }
    }




    public void OpenChest()
    {
        StartCoroutine(LidAction(112));
        interactedTooltip = "Chest opened";
        tooltip = "Press {0} to close!";
        open = true;
    }

    public void CloseChest()
    {
        
        StartCoroutine(LidAction(0));
        interactedTooltip = "Chest closed";
        tooltip = "Press {0} to open!";
        open = false;
    }


    void Start()
    {
        CanInteract = true;
    }

    IEnumerator LidAction(float target)
    {
        this.target = target;
        CanInteract = false;
        while (!current.Approx(target))
        {
            lid.transform.localEulerAngles = new Vector3(current, 0, 0);
            current = Mathf.MoveTowardsAngle(current, target, lidSpeed * Time.deltaTime);
            yield return null;
        }
        CanInteract = true;
        current = target;
        lid.transform.localEulerAngles = new Vector3(current, 0, 0);
        if(open)OnOpened?.Invoke();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    lid.transform.localEulerAngles = new Vector3(current, 0, 0);
    //    current = Mathf.MoveTowardsAngle(current, target, lidSpeed*Time.deltaTime);
    //}
}
