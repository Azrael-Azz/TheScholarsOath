using UnityEngine;
using System;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance { get; private set; }

    public event Action OnQuizStarted;
    public event Action<bool> OnQuizEnded;
    public event Action<Question> OnQuestionChanged;
    public event Action<bool> OnAnswerEvaluated;

    [Serializable]
    public class Question
    {
        public string prompt;
        public string[] options;
        public int correctIndex;
        public int difficulty;
        public string subject;
    }

    List<Question> currentQuestions = new List<Question>();
    int currentIndex = 0;
    int score = 0;
    bool active = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartQuiz(List<Question> questions)
    {
        if (active) return;

        currentQuestions = new List<Question>(questions);
        currentIndex = 0;
        score = 0;
        active = true;

        OnQuizStarted?.Invoke();
        PushQuestion();
        SoundManager.Instance.PlaySound(SoundManager.Instance.enemySound);
    }

    void PushQuestion()
    {
        if (!active) return;

        if (currentIndex >= currentQuestions.Count)
        {
            FinishQuiz();
            return;
        }

        OnQuestionChanged?.Invoke(currentQuestions[currentIndex]);
    }

    public void SubmitAnswer(int index)
    {
        if (!active) return;

        var q = currentQuestions[currentIndex];
        bool correct = index == q.correctIndex;
        if (correct) score++;

        OnAnswerEvaluated?.Invoke(correct);

        currentIndex++;
        PushQuestion();
        if (correct)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.correctSound);
        }
        else
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.wrongSound);
        }
    }

    void FinishQuiz()
    {
        active = false;

        float ratio = (currentQuestions.Count == 0) ? 0f : (float)score / currentQuestions.Count;
        bool passed = ratio >= 0.7f;

        OnQuizEnded?.Invoke(passed);
    }

    public bool IsActive() => active;
}