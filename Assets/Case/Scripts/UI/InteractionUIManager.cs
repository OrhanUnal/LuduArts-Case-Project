using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace InteractionSystem.Runtime.UI
{
    public class InteractionUIManager : MonoBehaviour
    {
        #region Fields

        public static InteractionUIManager Instance { get; private set; }

        [SerializeField] private GameObject m_InteractionPromptPanel;
        [SerializeField] private TextMeshProUGUI m_InteractionText;

        [SerializeField] private GameObject m_HoldProgressPanel;
        [SerializeField] private Image m_ProgressBarFill;

        [SerializeField] private GameObject m_AlertPanel;
        [SerializeField] private TextMeshProUGUI m_AlertText;
        [SerializeField] private float m_AlertDuration = 2f;

        private Coroutine m_AlertCoroutine;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("Multiple InteractionUIManager detected. Destroying duplicate.");
                Destroy(gameObject);
                return;
            }

            HideAll();
        }

        #endregion

        #region Interaction Prompt

        public void ShowInteractionPrompt(string message)
        {
            if (m_InteractionPromptPanel == null || m_InteractionText == null)
            {
                Debug.LogError("Interaction prompt UI references are missing.");
                return;
            }

            m_InteractionPromptPanel.SetActive(true);
            m_InteractionText.text = message;
        }

        public void HideInteractionPrompt()
        {
            if (m_InteractionPromptPanel == null)
                return;

            m_InteractionPromptPanel.SetActive(false);
        }

        #endregion

        #region Hold Progress

        public void ShowHoldProgress()
        {
            if (m_HoldProgressPanel == null || m_ProgressBarFill == null)
            {
                Debug.LogError("Hold progress UI references are missing.");
                return;
            }

            m_HoldProgressPanel.SetActive(true);
            m_ProgressBarFill.fillAmount = 0f;
        }

        public void UpdateHoldProgress(float progress)
        {
            if (!m_HoldProgressPanel.activeSelf)
                return;

            m_ProgressBarFill.fillAmount = Mathf.Clamp01(progress);
        }

        public void HideHoldProgress()
        {
            if (m_HoldProgressPanel == null || m_ProgressBarFill == null)
                return;

            m_HoldProgressPanel.SetActive(false);
            m_ProgressBarFill.fillAmount = 0f;
        }

        #endregion

        #region Alerts

        public void ShowAlert(string message)
        {
            if (m_AlertPanel == null || m_AlertText == null)
            {
                Debug.LogError("Alert UI references are missing.");
                return;
            }

            if (m_AlertCoroutine != null)
            {
                StopCoroutine(m_AlertCoroutine);
            }

            m_AlertCoroutine = StartCoroutine(AlertCoroutine(message));
        }

        private IEnumerator AlertCoroutine(string message)
        {
            m_AlertPanel.SetActive(true);
            m_AlertText.text = message;

            yield return new WaitForSeconds(m_AlertDuration);

            m_AlertPanel.SetActive(false);
            m_AlertCoroutine = null;
        }

        #endregion

        #region Methods

        private void HideAll()
        {
            HideInteractionPrompt();
            HideHoldProgress();

            if (m_AlertPanel != null)
                m_AlertPanel.SetActive(false);
        }

        #endregion
    }
}