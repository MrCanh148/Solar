using UnityEngine;

public class AlienController : MonoBehaviour
{
    public enum BotState
    {
        RandomMovement,
        FollowEnemy
    }

    public BotState currentState = BotState.RandomMovement;
    [SerializeField] private AttackTarget attackScript;
    [SerializeField] private RandomMovement moveScript;
    [SerializeField] private ShootTarget shootTarget;

    void OnTriggerEnter2D(Collider2D other)
    {
        ShootTarget target = Cache.GetShootTargetCollider(other);
        Character tg = Cache.GetCharacterCollider(other);

        if (other.CompareTag(Constant.TAG_AirSpace1))
        {
            if (target.hostAlien != null && shootTarget.hostAlien != null && target.hostAlien.myFamily == shootTarget.hostAlien.myFamily)
                return;
            else
            {
                attackScript.SetTarget(other.transform);
                SwitchToFollowEnemy();
            }
        }

        if (tg != null && tg.characterType == CharacterType.LifePlanet)
        {
            if (shootTarget.hostAlien != null && tg.myFamily == shootTarget.hostAlien.myFamily) return;
            else
            {
                attackScript.SetTarget(other.transform);
                SwitchToFollowEnemy();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Constant.TAG_AirSpace1) || other.CompareTag(Constant.TAG_Planet))
        {
            SwitchToRandomMovement();
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case BotState.RandomMovement:
                moveScript.enabled = true;
                attackScript.enabled = false;
                break;
            case BotState.FollowEnemy:
                moveScript.enabled = false;
                attackScript.enabled = true;
                break;
        }
    }

    void SwitchToFollowEnemy()
    {
        currentState = BotState.FollowEnemy;
    }

    void SwitchToRandomMovement()
    {
        currentState = BotState.RandomMovement;
    }
}
