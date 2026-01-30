using UnityEngine;

public class ChestScript : MonoBehaviour, IInteractable
{
    void IInteractable.InteractLogicHold()
    {
        return;
    }

    void IInteractable.InteractLogicButton()
    {
        Debug.Log("TRYING TO OPEN CHES");
    }
}
