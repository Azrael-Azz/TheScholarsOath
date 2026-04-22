using UnityEngine;

public class EnemyQuizCombat : MonoBehaviour
{
    public int damageToPlayerOnFail = 20;
    public int xpReward = 10;
    private bool waitingForQuizResult = false;
    public QuizGate linkedGate;

    void Start()
    {
        if (QuizManager.Instance != null)
        {
            QuizManager.Instance.OnQuizEnded += HandleQuizEnded;
        }
    }

    void OnDestroy()
    {
        if (QuizManager.Instance != null)
        {
            QuizManager.Instance.OnQuizEnded -= HandleQuizEnded;
        }
    }

    public void BeginQuizCombat()
    {
        waitingForQuizResult = true;
        Debug.Log("Enemy is now waiting for quiz result.");
    }

    void HandleQuizEnded(bool passed)
    {
        if (!waitingForQuizResult) return;

        waitingForQuizResult = false;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (passed)
        {
            Debug.Log("Enemy defeated!");

            if (player != null)
            {
                PlayerXP xp = player.GetComponent<PlayerXP>();
                if (xp != null)
                {
                    xp.AddXP(xpReward);
                }
            }

            if (linkedGate != null)
            {
                linkedGate.OpenGate();
            }

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Quiz failed. Player takes damage.");

            if (player != null)
            {
                PlayerHealth health = player.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.TakeDamage(damageToPlayerOnFail);
                }
            }
        }
    }
}