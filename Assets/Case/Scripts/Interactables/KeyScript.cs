using UnityEngine;

public class KeyScript : MonoBehaviour, IInteractable
{
    string IInteractable.GetInteractionPrompt()
    {
        return $"[PRESS E] Take {m_CurrentType}";
    }
    public enum KeyTypes
    {
        redKey,
        greenKey, 
        blueKey,
        None
    }
    [SerializeField] private KeyTypes m_CurrentType = KeyTypes.None;

    void IInteractable.InteractLogicHold()
    {
        return;
    }
    void IInteractable.InteractLogicButton() 
    {
        InventoryManager.Instance.AddItem(m_CurrentType);
        Destroy(gameObject);
    }
}
