using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizUI : MonoBehaviour
{
    [Header("UI Refs")]
    public GameObject quizPanel;
    public TMP_Text questionText;
    public TMP_Text feedbackText;
    public Button[] optionButtons; // size 4

    QuizManager qm;

    void Awake()
    {
        qm = QuizManager.Instance != null ? QuizManager.Instance : FindFirstObjectByType<QuizManager>();
        quizPanel.SetActive(false);
        if (feedbackText != null) feedbackText.text = "";
    }

    void OnEnable()
    {
        if (qm == null) return;
        qm.OnQuizStarted += HandleQuizStarted;
        qm.OnQuizEnded += HandleQuizEnded;
        qm.OnQuestionChanged += HandleQuestionChanged;
        qm.OnAnswerEvaluated += HandleAnswerEvaluated;
    }

    void OnDisable()
    {
        if (qm == null) return;
        qm.OnQuizStarted -= HandleQuizStarted;
        qm.OnQuizEnded -= HandleQuizEnded;
        qm.OnQuestionChanged -= HandleQuestionChanged;
        qm.OnAnswerEvaluated -= HandleAnswerEvaluated;
    }

    void HandleQuizStarted()
    {
        quizPanel.SetActive(true);
        if (feedbackText != null) feedbackText.text = "";
    }

    void HandleQuizEnded(bool passed)
    {
        quizPanel.SetActive(false);
    }

    void HandleQuestionChanged(QuizManager.Question question)
    {
        questionText.text = question.prompt;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            Button button = optionButtons[i];

            if (i < question.options.Length)
            {
                button.gameObject.SetActive(true);

                int capturedIndex = i;
                string capturedOptionText = question.options[i];

                TMP_Text textComponent = button.GetComponentInChildren<TMP_Text>(true);
                textComponent.text = capturedOptionText;

                Debug.Log($"Setup: slot {capturedIndex} = {button.name}, text = {capturedOptionText}");

                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    Debug.Log("Clicked button index: " + capturedIndex);
                    Debug.Log("Button text shown: " + capturedOptionText);
                    qm.SubmitAnswer(capturedIndex);
                });
            }
            else
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    void HandleAnswerEvaluated(bool correct)
    {
        if (feedbackText != null)
            feedbackText.text = correct ? "Correct ✅" : "Wrong ❌";
    }
}