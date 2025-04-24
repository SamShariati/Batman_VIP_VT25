using UnityEngine;

public class Pillar : MonoBehaviour
{
    Animator animator;
    bool up;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Up()
    {
        animator.SetBool("IsUp", true);
        up = true;
    }

    public void Down()
    {
        animator.SetBool("IsUp", false);
        up = false;
    }

    public void Toggle()
    {
        if(up) Down();
        else Up();
    }

}
