using UnityEngine;

public class KeyScript : MonoBehaviour, IInteractable
{
    void IInteractable.InteractLogicHold()
    {
        return;
    }
    void IInteractable.InteractLogicButton() 
    {
        Debug.Log("TOOK KEY");
    }
}
