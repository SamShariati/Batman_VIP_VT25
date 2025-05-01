using System;
using UnityEngine;

public class KeyAnimatorCallbacks : MonoBehaviour
{
    public event Action OnThrow;
    public event Action OnThrowRock;

    public void ThrowKeyEvent()
    {
        OnThrow?.Invoke();
    }

    public void ThrowRockEvent()
    {
        OnThrowRock?.Invoke();
    }
}
