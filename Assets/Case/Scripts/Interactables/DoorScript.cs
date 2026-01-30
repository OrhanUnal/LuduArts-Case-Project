using InteractionSystem.Runtime.UI;
using System.Collections;
using TMPro;
using UnityEngine;
namespace InteractionSystem.Runtime.Interactables
{
    /// <summary>
    /// Door interactable implementation.
    /// Supports toggle, hold-to-unlock and locked state logic.
    /// </summary>
    public class DoorInteractable : MonoBehaviour, IInteractable
    {
        #region Fields
        private enum stateOfDoor
        {
            Closed,
            InAnimation,
            Open
        }

        [SerializeField] private bool m_IsLocked = false;
        [SerializeField] private KeyScript.KeyTypes m_RequiredKeyType = KeyScript.KeyTypes.None;
        [SerializeField] private Transform m_HingeTransform;
        [SerializeField] private TextMeshProUGUI m_InteractResponse;

        private const float k_AnimationDuration = 1.5f;
        private const float k_AlertMessageTimer = 1.3f;
        
        private stateOfDoor m_CurrentState = stateOfDoor.Closed;
        #endregion

        private void OnEnable()
        {
            LeverScript.OpenEveryDoor += HandleLever;
        }

        private void OnDisable()
        {
            LeverScript.OpenEveryDoor -= HandleLever;
        }

        #region Interface Implementations
        string IInteractable.GetInteractionPrompt()
        {
            if (m_IsLocked)
                return $"[Hold E] Unlock Door ({m_RequiredKeyType} required)";
            else if (m_CurrentState == stateOfDoor.Open)
                return "[E] Close Door";
            else
                return "[E] Open Door";
        }
        void IInteractable.InteractLogicButton()
        {
            if (m_CurrentState == stateOfDoor.InAnimation)
                return;
            if (!m_IsLocked)
                ToggleDoor();
            else
                StartCoroutine(HoldToOpen());
        }
        void IInteractable.InteractLogicHold()
        {
            if (m_IsLocked)
            {
                TryUnlockDoor();
            }
            else
            {
                OpenDoor();
            }
        }
        #endregion
        #region Methods

        private void HandleLever()
        {
            if (m_CurrentState != stateOfDoor.InAnimation && !m_IsLocked)
                ToggleDoor();
        }

        private void TryUnlockDoor()
        {
            if (InventoryManager.Instance.HasItem(m_RequiredKeyType))
            {
                m_IsLocked = false;
                InventoryManager.Instance.RemoveItem(m_RequiredKeyType);
                OpenDoor();
                InteractionUIManager.Instance.ShowAlert("Openning the door");
            }
            else
            {
                InteractionUIManager.Instance.ShowAlert($"You need to {m_RequiredKeyType} to unlock this door");
            }
        }
        private void OpenDoor()
        {
            if (m_CurrentState == stateOfDoor.Open || m_CurrentState == stateOfDoor.InAnimation)
                return;
            StartCoroutine(AnimateDoor(1, stateOfDoor.Open));
        }
        private void CloseDoor()
        {
            if (m_CurrentState == stateOfDoor.Closed || m_CurrentState == stateOfDoor.InAnimation)
                return;
            StartCoroutine(AnimateDoor(-1, stateOfDoor.Closed));
        }
        private void ToggleDoor()
        {
            if (m_CurrentState == stateOfDoor.Open)
                CloseDoor();
            else
                OpenDoor();
        }

        private IEnumerator HoldToOpen()
        {
            float m_HoldProgress = 0f;
            float m_HoldDuration = 2f;
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
        private IEnumerator AnimateDoor(int direction, stateOfDoor targetState)
        {
            m_CurrentState = stateOfDoor.InAnimation;
            Quaternion startRotation = m_HingeTransform.rotation;
            Quaternion endRotation = startRotation * Quaternion.Euler(0f, 90f * direction, 0f);
            float elapsedTime = 0f;
            while (elapsedTime < k_AnimationDuration)
            {
                m_HingeTransform.rotation = Quaternion.Slerp(
                    startRotation,
                    endRotation,
                    elapsedTime / k_AnimationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            m_HingeTransform.rotation = endRotation;
            m_CurrentState = targetState; // Set the final state after animation completes
        }
        #endregion
    }
}