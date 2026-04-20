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

    void HandleQuestionChanged(QuizManager.Question q)
    {
        questionText.text = q.prompt;

        // safety: if options < buttons, disable extras
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < q.options.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<TMP_Text>().text = q.options[i];

                int captured = i;
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => qm.SubmitAnswer(captured));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void HandleAnswerEvaluated(bool correct)
    {
        if (feedbackText != null)
            feedbackText.text = correct ? "Correct ✅" : "Wrong ❌";
    }
}