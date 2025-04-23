using UnityEngine;

public abstract class HudBase : MonoBehaviour
{
    
    public virtual void Awake()
    {
        Hud hud = GetComponentInParent<Hud>();
        if (hud)
        {
            hud.OnPlayerObject += Hud_OnPlayerObject;
        }
    }

    private void Hud_OnPlayerObject(GameObject obj)
    {
        if (obj)
        {
            OnPlayerObject(obj);
        }
        else
        {
            OnNoPlayer();
        }
    }

    public abstract void OnPlayerObject(GameObject obj);

    public abstract void OnNoPlayer();
}
