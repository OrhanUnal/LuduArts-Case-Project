using UnityEngine;

public class DoorScript : MonoBehaviour, IInteractable
{
    void IInteractable.InteractLogicButton()
    {
        Debug.Log("START OPENING DOOR BAR");
    }

    void IInteractable.InteractLogicHold()
    {
        Debug.Log("OPEN DOOR");
    }

}
