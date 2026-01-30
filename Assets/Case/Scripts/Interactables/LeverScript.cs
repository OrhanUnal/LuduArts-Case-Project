using System;
using UnityEngine;

public class LeverScript : MonoBehaviour, IInteractable
{
    public static event Action OpenEveryDoor;
    void IInteractable.InteractLogicHold()
    {
        return;
    }

    void IInteractable.InteractLogicButton()
    {
        OpenEveryDoor?.Invoke();
    }
}
