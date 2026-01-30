using InteractionSystem.Runtime.UI;
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
    [SerializeField] private float m_HoldDuration = 2f;
    [SerializeField] private float m_OpenAngle = 110f;
    [SerializeField] private float m_OpenDuration = 1.5f;

    private bool m_IsOpen;
    private bool m_IsAnimating;
    private float m_HoldProgress = 0f;

    #endregion

    #region Interface Implementations
    string IInteractable.GetInteractionPrompt()
    {
        return "[Hold E] Open Chest";
    }
    void IInteractable.InteractLogicButton()
    {
        StartCoroutine(HoldToOpen());
    }

    void IInteractable.InteractLogicHold()
    {
        if (m_IsOpen || m_IsAnimating)
            return;

        StartCoroutine(OpenChestCoroutine());
    }

    #endregion

    #region Methods
    private IEnumerator HoldToOpen()
    {
        m_HoldProgress = 0f;
        InteractionUIManager.Instance.ShowHoldProgress();

        while (m_HoldProgress < m_HoldDuration)
        {
            m_HoldProgress += Time.deltaTime;
            float progress = m_HoldProgress / m_HoldDuration;
            InteractionUIManager.Instance.UpdateHoldProgress(progress);
            yield return null;
        }

        InteractionUIManager.Instance.HideHoldProgress();

    }

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