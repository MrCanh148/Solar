using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    [SerializeField] private float wanderRadius = 5f;
    [SerializeField] private float changeDirectionInterval = 2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 50f;
    private float changeDirectionTimer;
    private Vector2 targetPosition;
    private Rigidbody2D rb;
    private Vector2 currentDirection;


    public enum AirSpaceType
    {
        Fighter = 0,
        Cruiser = 1,
        MissileBoat = 2,
    };

    public AirSpaceType type;
    [HideInInspector] public Transform centerPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetRandomTargetPosition();
    }

    void Update()
    {
        changeDirectionTimer -= Time.deltaTime;

        if (changeDirectionTimer <= 0)
        {
            SetRandomTargetPosition();
            changeDirectionTimer = changeDirectionInterval;
        }

        Move();
    }

    void SetRandomTargetPosition()
    {
        if (centerPoint == null)
        {
            targetPosition = (Vector2)transform.position + Random.insideUnitCircle.normalized * wanderRadius;
        }
        else
        {
            Vector2 randomDirection = Random.insideUnitCircle * wanderRadius;
            targetPosition = (Vector2)centerPoint.position + randomDirection;
        }
    }

    private void Move()
    {
        Vector2 targetDirection = (targetPosition - (Vector2)transform.position).normalized;
        currentDirection = Vector2.Lerp(currentDirection, targetDirection, Time.deltaTime * moveSpeed);
        rb.velocity = currentDirection * moveSpeed;

        float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        if (Vector2.Distance(transform.position, targetPosition) < 0.5f)
        {
            SetRandomTargetPosition();
        }
    }

    public void SetCenterPoint(Transform newCenterPoint)
    {
        centerPoint = newCenterPoint;
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
