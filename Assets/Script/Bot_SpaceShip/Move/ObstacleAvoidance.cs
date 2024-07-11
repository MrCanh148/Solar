using UnityEngine;

public class ObstacleAvoidance : MonoBehaviour
{
    private RandomMovement parentRandomMovement;
    private AttackTarget parentAttackTarget;
    private float lastAvoidTime;
    [SerializeField] private float avoidCooldown = 2f;

    void Start()
    {
        parentRandomMovement = GetComponentInParent<RandomMovement>();
        parentAttackTarget = GetComponentInParent<AttackTarget>();
        lastAvoidTime = -avoidCooldown;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        ShootTarget tg = collision.GetComponent<ShootTarget>();

        if ((character != null || tg != null) && Time.time >= lastAvoidTime + avoidCooldown)
        {
            if (parentRandomMovement != null)
                parentRandomMovement.AvoidObstacle();

            if (parentAttackTarget != null)
                parentAttackTarget.AvoidObstacle();

            lastAvoidTime = Time.time;
        }
    }
}
