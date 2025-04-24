using System.Collections.Generic;
using UnityEngine;

public class TempKeyInventory : MonoBehaviour
{
    
    //this is perhaps too much responsibility breaking SRP
    public Animator animator;


    HashSet<string> keys = new();

    void Start()
    {
        
    }

    public void UseKey()
    {
        animator.SetTrigger("UseKey");
    }

    public void PickUpAnim()
    {
        animator.SetBool("HasKey", true);
        animator.SetTrigger("PickUpKey");
    }

    

    public bool HasKey(string key)
    {
        return keys.Contains(key);
    }

    public bool PickUpKey(string key)
    {
        return keys.Add(key);
    }
}
