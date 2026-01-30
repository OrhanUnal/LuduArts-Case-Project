using UnityEngine;

public class KeyScript : MonoBehaviour, IInteractable
{
    public enum KeyTypes
    {
        redKey,
        greenKey, 
        blueKey,
        None
    }

    [SerializeField] private KeyTypes m_CurrentType = KeyTypes.None;
    [SerializeField] private MeshRenderer m_KeyRenderer;

    private void Start()
    {
        UpdateKeyColor();
    }

    string IInteractable.GetInteractionPrompt()
    {
        return $"[PRESS E] Take {m_CurrentType}";
    }
    void IInteractable.InteractLogicHold()
    {
        return;
    }
    void IInteractable.InteractLogicButton() 
    {
        InventoryManager.Instance.AddItem(m_CurrentType);
        Destroy(gameObject);
    }

    private void UpdateKeyColor()
    {
        if (m_KeyRenderer == null)
        {
            m_KeyRenderer = GetComponentInChildren<MeshRenderer>();
        }

        if (m_KeyRenderer != null)
        {
            Color keyColor = GetColorForKeyType(m_CurrentType);
            
            Material keyMaterial = new Material(m_KeyRenderer.sharedMaterial);
            keyMaterial.color = keyColor;
            m_KeyRenderer.material = keyMaterial;
        }
    }

    private Color GetColorForKeyType(KeyTypes keyType)
    {
        switch (keyType)
        {
            case KeyTypes.redKey:
                return Color.red;
            case KeyTypes.blueKey:
                return Color.blue;
            case KeyTypes.greenKey:
                return Color.green;
            default:
                return Color.white;
        }
    }
}
