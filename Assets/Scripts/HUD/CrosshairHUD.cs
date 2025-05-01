using UnityEngine;

public class CrosshairHUD : HudBase
{

    Interactor  interactor;
    [SerializeField] Animator animator;

    public override void OnNoPlayer()
    {
        enabled = false; 
        if(interactor)
            interactor.CanInteract -= Interactor_CanInteract;
    }

    public override void OnPlayerObject(GameObject obj)
    {
        if (obj)
        {
            interactor = obj.GetComponent<Interactor>();
            enabled = false;
            interactor.CanInteract += Interactor_CanInteract;
        }
        
    }

    private void Interactor_CanInteract(bool obj)
    {
        if(animator)animator.SetBool("CanInteract", obj);
    }

    
    void Start()
    {
        if (!animator) animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
