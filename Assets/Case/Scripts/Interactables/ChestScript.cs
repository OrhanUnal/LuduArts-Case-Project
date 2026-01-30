using System.Collections;
using UnityEngine;

/// <summary>
/// Chest interactable. Opens with hold interaction.
/// Can only be opened once.
/// </summary>
public class ChestScript : MonoBehaviour, IInteractable
{
    #region Fields

    [SerializeField] private Transform m_HingeTransform;
    [SerializeField] private float m_OpenAngle = 110f;
    [SerializeField] private float m_OpenDuration = 1.5f;

    private bool m_IsOpen;
    private bool m_IsAnimating;

    #endregion

    #region Interface Implementations

    void IInteractable.InteractLogicButton()
    {
        Debug.Log("Chest requires hold interaction.");
    }

    void IInteractable.InteractLogicHold()
    {
        if (m_IsOpen || m_IsAnimating)
            return;

        StartCoroutine(OpenChestCoroutine());
    }

    #endregion

    #region Methods

    private IEnumerator OpenChestCoroutine()
    {
        m_IsAnimating = true;

        Quaternion startRotation = m_HingeTransform.localRotation;
        Quaternion endRotation =
            startRotation * Quaternion.Euler(m_OpenAngle, 0f, 0f);

        float elapsed = 0f;

        while (elapsed < m_OpenDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / m_OpenDuration;
            m_HingeTransform.localRotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        m_HingeTransform.localRotation = endRotation;
        m_IsOpen = true;
        m_IsAnimating = false;
    }

    #endregion
}