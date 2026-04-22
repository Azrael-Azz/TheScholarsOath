using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class QuestionApiService : MonoBehaviour
{
    public static QuestionApiService Instance { get; private set; }

    [SerializeField] private string baseUrl = "http://localhost:5161/api/questions";

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

    public void GetQuestions(string subject, string difficulty, string area, int count,
        Action<List<QuizManager.Question>> onSuccess,
        Action<string> onError)
    {
        StartCoroutine(GetQuestionsCoroutine(subject, difficulty, area, count, onSuccess, onError));
    }

    private IEnumerator GetQuestionsCoroutine(string subject, string difficulty, string area, int count,
        Action<List<QuizManager.Question>> onSuccess,
        Action<string> onError)
    {
        string url =
            $"{baseUrl}/random?subject={UnityWebRequest.EscapeURL(subject)}&difficulty={UnityWebRequest.EscapeURL(difficulty)}&area={UnityWebRequest.EscapeURL(area)}&count={count}";

        using UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            onError?.Invoke(request.error);
            yield break;
        }

        string wrappedJson = "{\"questions\":" + request.downloadHandler.text + "}";
        ApiQuestionList apiResult = JsonUtility.FromJson<ApiQuestionList>(wrappedJson);

        List<QuizManager.Question> quizQuestions = new List<QuizManager.Question>();

        foreach (var q in apiResult.questions)
        {
            quizQuestions.Add(new QuizManager.Question
            {
                prompt = q.prompt,
                options = new string[] { q.optionA, q.optionB, q.optionC, q.optionD },
                correctIndex = q.correctIndex
            });
        }

        onSuccess?.Invoke(quizQuestions);
    }
}