using UnityEngine;

public class AttackTarget : MonoBehaviour
{
    private Transform target;
    private Rigidbody2D rb;
    private Vector2 currentDirection;
    private float moveSpeed = 3f;
    private float rotationSpeed = 50f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (target != null)
        {
            MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget()
    {
        Vector2 targetDirection = (target.position - transform.position).normalized;
        currentDirection = Vector2.Lerp(currentDirection, targetDirection, Time.deltaTime * moveSpeed);
        rb.velocity = currentDirection * moveSpeed;

        float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void AvoidObstacle()
    {
        currentDirection = Quaternion.Euler(0, 0, 60) * currentDirection;
        rb.velocity = currentDirection * moveSpeed;

        float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
