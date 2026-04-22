using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class QuizTrigger : MonoBehaviour
{
    public bool startOnEnter = false;
    public string promptText = "Press E to start quiz";

    [Header("API Settings")]
    public string subject = "Math";
    public string difficulty = "Easy";
    public string area = "Start";
    public int questionCount = 3;

    bool playerInRange = false;

    void Update()
    {
        if (!playerInRange) return;
        if (QuizManager.Instance != null && QuizManager.Instance.IsActive()) return;

        if (startOnEnter)
        {
            StartQuiz();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            StartQuiz();
        }
    }

    void StartQuiz()
    {
        if (QuizManager.Instance == null)
        {
            Debug.LogWarning("No QuizManager present in the scene.");
            return;
        }

        if (QuestionApiService.Instance == null)
        {
            Debug.LogWarning("No QuestionApiService present in the scene.");
            return;
        }

        EnemyQuizCombat enemyCombat = GetComponent<EnemyQuizCombat>();
        if (enemyCombat != null)
        {
            enemyCombat.BeginQuizCombat();
        }

        QuestionApiService.Instance.GetQuestions(
            subject,
            difficulty,
            area,
            questionCount,
            quizQuestions =>
            {
                if (quizQuestions.Count == 0)
                {
                    Debug.LogWarning("No questions returned from API.");
                    return;
                }

                QuizManager.Instance.StartQuiz(quizQuestions);
            },
            error =>
            {
                Debug.LogError("API error: " + error);
            }
        );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log(promptText);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}