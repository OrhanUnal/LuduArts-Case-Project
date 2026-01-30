using UnityEngine;

public class KeyScript : MonoBehaviour, IInteractable
{
    public enum KeyType
    {
        Red,
        Green,
        Blue,
        None
    }

    #region Fields

    [SerializeField] private KeyType m_CurrentType = KeyType.None;
    [SerializeField] private MeshRenderer m_KeyRenderer;

    #endregion

    #region Unity Methods

    private void Start()
    {
        UpdateKeyColor();
    }

    #endregion

    #region Interface Implementations

    string IInteractable.GetInteractionPrompt()
    {
        return $"[Press E] Take {m_CurrentType}";
    }

    void IInteractable.InteractLogicHold()
    {
        Debug.Log("Key does not support hold interaction.");
    }

    void IInteractable.InteractLogicButton()
    {
        InventoryManager.Instance.AddItem(m_CurrentType);
        Destroy(gameObject);
    }

    #endregion

    #region Methods

    private void UpdateKeyColor()
    {
        if (m_KeyRenderer == null)
        {
            m_KeyRenderer = GetComponentInChildren<MeshRenderer>();
        }

        if (m_KeyRenderer == null)
        {
            Debug.LogError("Key renderer not found.");
            return;
        }

        Material keyMaterial = new Material(m_KeyRenderer.sharedMaterial)
        {
            color = GetColorForKeyType(m_CurrentType)
        };

        m_KeyRenderer.material = keyMaterial;
    }

    private static Color GetColorForKeyType(KeyType keyType)
    {
        return keyType switch
        {
            KeyType.Red => Color.red,
            KeyType.Green => Color.green,
            KeyType.Blue => Color.blue,
            _ => Color.white
        };
    }

    #endregion
}