using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    [SerializeField] private List<KeyScript.KeyTypes> m_Keys = new List<KeyScript.KeyTypes>();

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

    public bool HasItem(KeyScript.KeyTypes type)
    {
        return m_Keys.Contains(type);
    }

    public void AddItem(KeyScript.KeyTypes type)
    {
        m_Keys.Add(type);
    }

    public void RemoveItem(KeyScript.KeyTypes type)
    {
        if (m_Keys.Contains(type))
            m_Keys.Remove(type);
    }
}