using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace InteractionSystem.Runtime.UI
{
    public class InteractionUIManager : MonoBehaviour
    {
        public static InteractionUIManager Instance { get; private set; }
        [SerializeField] private GameObject m_InteractionPromptPanel;
        [SerializeField] private TextMeshProUGUI m_InteractionText;
        [SerializeField] private GameObject m_HoldProgressPanel;
        [SerializeField] private Image m_ProgressBarFill;
        [SerializeField] private GameObject m_AlertPanel;
        [SerializeField] private TextMeshProUGUI m_AlertText;
        [SerializeField] private float m_AlertDuration = 2f;

        private Coroutine m_AlertCoroutine;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            HideAll();
        }

        #region Interaction Prompt
        public void ShowInteractionPrompt(string message)
        {
            m_InteractionPromptPanel.SetActive(true);
            m_InteractionText.text = message;
        }

        public void HideInteractionPrompt()
        {
            m_InteractionPromptPanel.SetActive(false);
        }
        #endregion

        #region Hold Progress Bar
        public void ShowHoldProgress()
        {
            m_HoldProgressPanel.SetActive(true);
            m_ProgressBarFill.fillAmount = 0f;
        }

        public void UpdateHoldProgress(float progress)
        {
            if (m_HoldProgressPanel.activeSelf)
            {
                m_ProgressBarFill.fillAmount = Mathf.Clamp01(progress);
            }
        }

        public void HideHoldProgress()
        {
            m_HoldProgressPanel.SetActive(false);
            m_ProgressBarFill.fillAmount = 0f;
        }
        #endregion

        #region Alert Messages
        public void ShowAlert(string message)
        {
            if (m_AlertCoroutine != null)
            {
                StopCoroutine(m_AlertCoroutine);
            }

            m_AlertCoroutine = StartCoroutine(ShowAlertCoroutine(message));
        }

        private IEnumerator ShowAlertCoroutine(string message)
        {
            m_AlertPanel.SetActive(true);
            m_AlertText.text = message;

            yield return new WaitForSeconds(m_AlertDuration);

            m_AlertPanel.SetActive(false);
            m_AlertCoroutine = null;
        }
        #endregion

        private void HideAll()
        {
            HideInteractionPrompt();
            HideHoldProgress();
            if (m_AlertPanel != null)
                m_AlertPanel.SetActive(false);
        }
    }
}