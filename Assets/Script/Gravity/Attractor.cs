using UnityEngine;

public class Attractor : MonoBehaviour
{
    float G = 0.03337f;
    public Character owner;
    Character otherAttractor;

    private int coefficient;
    private Vector3 direction, force;
    private float distance, forceMagnitude;

    private void OnTriggerStay2D(Collider2D collision)
    {
        otherAttractor = Cache.GetCharacterCollider(collision);

        if (otherAttractor != null && owner != null)
        {
            Attract(owner, otherAttractor);
        }
    }

    private void Attract(Character attractor, Character target)
    {
        if (attractor.characterType > CharacterType.Asteroid)
        {
            if (attractor.characterType >= target.characterType && target.host == null)
            {
                if (attractor.characterType - target.characterType > 0)
                    coefficient = attractor.characterType - target.characterType;
                else
                    coefficient = 1;

                direction = attractor.tf.position - target.tf.position;
                distance = direction.magnitude;

                if (distance <= 0.1f)
                {
                    return;
                }

                forceMagnitude = (G * coefficient) / Mathf.Pow(distance, 2);
                force = direction.normalized * forceMagnitude;

                if (target.isPlayer)
                    target.velocity += (Vector2)force;
                else
                    target.externalVelocity += (Vector2)force;
            }
        }
    }
}
