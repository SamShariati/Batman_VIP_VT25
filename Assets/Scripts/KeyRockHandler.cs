using System.Collections.Generic;
using UnityEngine;

/**
 * Im gon hijack ths to handle key hands as we skip inventory, prep for re-maning in future
 * 
 */
public class KeyRockHandler : MonoBehaviour
{
    
    //this is perhaps too much responsibility breaking SRP
    public Animator animator;
    public Key keyPrefab;
    public Rock rockPrefab;
    public ThrowObject throwObject;

    bool holding;
    string currentKey;
    bool holdingRock;
    readonly HashSet<string> keys = new();

    void Start()
    {
        if (animator)
        {

            animator.GetComponent<KeyAnimatorCallbacks>().OnThrow += ThrowKey;
            animator.GetComponent<KeyAnimatorCallbacks>().OnThrowRock += ThrowRock;
        }
        if(!throwObject) throwObject = gameObject.GetOrAdd<ThrowObject>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && holding)
        {
            animator.SetTrigger("ThrowKey");
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && holdingRock)
        {
            animator.SetTrigger("ThrowRock");
        }
    }

    public void UseKey()
    {
        animator.SetTrigger("UseKey");
    }

    public void PickUpAnim()
    {
        animator.SetBool("HasKey", true);
        animator.SetTrigger("PickUpKey");
        holding = true;
    }

    public void ThrowKey()
    {
        holding = false;
        animator.SetBool("HasKey", false);
        Key key = throwObject.DoThrow(keyPrefab);
        key.keyColour = currentKey;
        DropKey(currentKey);
        //string current key??
    }

    public void ThrowRock()
    {
        holdingRock = false;
        throwObject.DoThrow(rockPrefab);
    }

    public bool HasKey(string key)
    {
        return keys.Contains(key);
    }

    public bool PickUpKey(string key)
    {
        currentKey = key;
        return keys.Add(key);
    }

    public bool DropKey(string key)
    {
        currentKey = string.Empty;
        return keys.Remove(key);
    }

    public bool PickUpRock()
    {
        animator.SetTrigger("PickUpRock");
        holdingRock = true;
        return true;
    }
}
