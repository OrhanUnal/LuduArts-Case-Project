using System;
using UnityEngine;

public class LeverScript : MonoBehaviour, IInteractable
{
    public static event Action OpenEveryDoor;

    string IInteractable.GetInteractionPrompt()
    {
        return "[Press E] Toggle Lever";
    }

    void IInteractable.InteractLogicHold()
    {
        Debug.Log("Lever does not support hold interaction.");
    }

    void IInteractable.InteractLogicButton()
    {
        OpenEveryDoor?.Invoke();
    }
}