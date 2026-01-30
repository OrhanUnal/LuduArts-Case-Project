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
        return;
    }

    void IInteractable.InteractLogicButton()
    {
        OpenEveryDoor?.Invoke();
    }
}
