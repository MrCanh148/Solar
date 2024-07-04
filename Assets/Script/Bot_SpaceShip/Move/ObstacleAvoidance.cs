using UnityEngine;

public class ObstacleAvoidance : MonoBehaviour
{
    private RandomMovement parentRandomMovement;
    private AttackTarget parentAttackTarget;

    void Start()
    {
        parentRandomMovement = GetComponentInParent<RandomMovement>();
        parentAttackTarget = GetComponentInParent<AttackTarget>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        ShootTarget tg = collision.GetComponent<ShootTarget>();

        if ((character != null || tg != null))
        {
            if (parentRandomMovement != null)
                parentRandomMovement.AvoidObstacle();

            if (parentAttackTarget != null)
                parentAttackTarget.AvoidObstacle();
        }
    }

}
