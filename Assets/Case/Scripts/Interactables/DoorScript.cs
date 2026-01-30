using System.Collections;
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
        private const float k_AnimationDuration = 1.5f;
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
        void IInteractable.InteractLogicButton()
        {
            if (m_CurrentState == stateOfDoor.InAnimation)
                return;
            if (m_IsLocked)
                Debug.Log("Door is locked. Hold to try unlocking.");
            else
                ToggleDoor();
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
                Debug.Log("Door unlocked!");
                InventoryManager.Instance.RemoveItem(m_RequiredKeyType);
                OpenDoor();
            }
            else
            {
                Debug.Log($"You need {m_RequiredKeyType} to unlock this door.");
            }
        }
        private void OpenDoor()
        {
            if (m_CurrentState == stateOfDoor.Open || m_CurrentState == stateOfDoor.InAnimation)
                return;
            Debug.Log("DOOR OPENING");
            StartCoroutine(AnimateDoor(1, stateOfDoor.Open));
        }
        private void CloseDoor()
        {
            if (m_CurrentState == stateOfDoor.Closed || m_CurrentState == stateOfDoor.InAnimation)
                return;
            Debug.Log("DOOR CLOSING");
            StartCoroutine(AnimateDoor(-1, stateOfDoor.Closed));
        }
        private void ToggleDoor()
        {
            if (m_CurrentState == stateOfDoor.Open)
                CloseDoor();
            else
                OpenDoor();
        }
        private IEnumerator AnimateDoor(int direction, stateOfDoor targetState)
        {
            m_CurrentState = stateOfDoor.InAnimation;
            Quaternion startRotation = transform.rotation;
            Quaternion endRotation =
                startRotation * Quaternion.Euler(0f, 90f * direction, 0f);
            float elapsedTime = 0f;
            while (elapsedTime < k_AnimationDuration)
            {
                transform.rotation = Quaternion.Slerp(
                    startRotation,
                    endRotation,
                    elapsedTime / k_AnimationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.rotation = endRotation;
            m_CurrentState = targetState; // Set the final state after animation completes
        }
        #endregion
    }
}