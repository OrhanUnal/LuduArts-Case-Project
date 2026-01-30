using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    [SerializeField] private List<KeyScript.KeyType> m_Keys = new List<KeyScript.KeyType>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool HasItem(KeyScript.KeyType type)
    {
        return m_Keys.Contains(type);
    }

    public void AddItem(KeyScript.KeyType type)
    {
        m_Keys.Add(type);
    }

    public void RemoveItem(KeyScript.KeyType type)
    {
        if (m_Keys.Contains(type))
            m_Keys.Remove(type);
    }
}