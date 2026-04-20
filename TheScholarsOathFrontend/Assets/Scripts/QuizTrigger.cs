using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class QuizTrigger : MonoBehaviour
{
    public bool startOnEnter = false;
    public QuizManager.Question[] questions;
    public string promptText = "Press E to start quiz";

    bool playerInRange = false;


    void Update()
    {
        if (!playerInRange) return;
        if (QuizManager.Instance != null && QuizManager.Instance.IsActive()) return;

        if (startOnEnter)
        {
            StartQuiz();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
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

        EnemyQuizCombat enemyCombat = GetComponent<EnemyQuizCombat>();
        if (enemyCombat != null)
        {
            enemyCombat.BeginQuizCombat();
        }

        var qList = new List<QuizManager.Question>(questions);
        QuizManager.Instance.StartQuiz(qList);
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
