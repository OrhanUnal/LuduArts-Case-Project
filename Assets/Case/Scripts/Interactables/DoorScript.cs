using InteractionSystem.Runtime.UI;
using System.Collections;
using TMPro;
using UnityEngine;

namespace InteractionSystem.Runtime.Interactables
{
    public class DoorInteractable : MonoBehaviour, IInteractable
    {
        #region Fields

        private enum DoorState
        {
            Closed,
            InAnimation,
            Open
        }

        [SerializeField] private bool m_IsLocked = false;
        [SerializeField] private KeyScript.KeyType m_RequiredKeyType = KeyScript.KeyType.None;
        [SerializeField] private Transform m_HingeTransform;
        [SerializeField] private TextMeshProUGUI m_InteractResponse;

        private const float k_AnimationDuration = 1.5f;
        private const float k_AlertMessageTimer = 1.3f;

        private DoorState m_CurrentState = DoorState.Closed;

        #endregion

        #region Unity Events

        private void OnEnable()
        {
            LeverScript.OpenEveryDoor += HandleLever;
        }

        private void OnDisable()
        {
            LeverScript.OpenEveryDoor -= HandleLever;
        }

        #endregion

        #region Interface Implementations

        string IInteractable.GetInteractionPrompt()
        {
            if (m_IsLocked)
                return $"[Hold E] Unlock Door ({m_RequiredKeyType} required)";

            if (m_CurrentState == DoorState.Open)
                return "[E] Close Door";

            return "[E] Open Door";
        }

        void IInteractable.InteractLogicButton()
        {
            if (m_CurrentState == DoorState.InAnimation)
            {
                Debug.LogWarning("Door interaction ignored: animation in progress.");
                return;
            }

            if (!m_IsLocked)
                ToggleDoor();
            else
                StartCoroutine(HoldToOpen());
        }

        void IInteractable.InteractLogicHold()
        {
            if (m_IsLocked)
                TryUnlockDoor();
            else
                OpenDoor();
        }

        #endregion

        #region Methods

        private void HandleLever()
        {
            if (m_CurrentState == DoorState.InAnimation)
            {
                Debug.LogWarning("Lever ignored: door animation in progress.");
                return;
            }

            if (!m_IsLocked)
                ToggleDoor();
        }

        private void TryUnlockDoor()
        {
            if (InventoryManager.Instance.HasItem(m_RequiredKeyType))
            {
                m_IsLocked = false;
                InventoryManager.Instance.RemoveItem(m_RequiredKeyType);
                OpenDoor();
                InteractionUIManager.Instance.ShowAlert("Opening the door");
            }
            else
            {
                InteractionUIManager.Instance.ShowAlert(
                    $"You need {m_RequiredKeyType} to unlock this door");
            }
        }

        private void OpenDoor()
        {
            if (m_CurrentState == DoorState.Open || m_CurrentState == DoorState.InAnimation)
            {
                Debug.LogWarning("OpenDoor ignored: invalid state.");
                return;
            }

            StartCoroutine(AnimateDoor(1, DoorState.Open));
        }

        private void CloseDoor()
        {
            if (m_CurrentState == DoorState.Closed || m_CurrentState == DoorState.InAnimation)
            {
                Debug.LogWarning("CloseDoor ignored: invalid state.");
                return;
            }

            StartCoroutine(AnimateDoor(-1, DoorState.Closed));
        }

        private void ToggleDoor()
        {
            if (m_CurrentState == DoorState.Open)
                CloseDoor();
            else
                OpenDoor();
        }

        private IEnumerator HoldToOpen()
        {
            float holdProgress = 0f;
            float holdDuration = 2f;

            InteractionUIManager.Instance.ShowHoldProgress();

            while (holdProgress < holdDuration)
            {
                holdProgress += Time.deltaTime;
                InteractionUIManager.Instance.UpdateHoldProgress(holdProgress / holdDuration);
                yield return null;
            }

            InteractionUIManager.Instance.HideHoldProgress();
        }

        private IEnumerator AnimateDoor(int direction, DoorState targetState)
        {
            m_CurrentState = DoorState.InAnimation;

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
            m_CurrentState = targetState;
        }

        #endregion
    }
}