using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;

    private Transform targetPoint;
    private bool canMove = true;

    void Start()
    {
        targetPoint = pointB;

        if (QuizManager.Instance != null)
        {
            QuizManager.Instance.OnQuizStarted += StopMoving;
            QuizManager.Instance.OnQuizEnded += ResumeMoving;
        }
    }

    void OnDestroy()
    {
        if (QuizManager.Instance != null)
        {
            QuizManager.Instance.OnQuizStarted -= StopMoving;
            QuizManager.Instance.OnQuizEnded -= ResumeMoving;
        }
    }

    void Update()
    {
        if (!canMove) return;
        if (pointA == null || pointB == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPoint.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            targetPoint = targetPoint == pointA ? pointB : pointA;
            Flip();
        }
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    void StopMoving()
    {
        canMove = false;
    }

    void ResumeMoving(bool passed)
    {
        canMove = true;
    }
}