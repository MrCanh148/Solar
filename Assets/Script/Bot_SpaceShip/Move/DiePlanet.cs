using System.Collections;
using UnityEngine;

public class DiePlanet : MonoBehaviour
{
    private bool isInvincible = true;
    private float invincibilityDuration = 1f; // Thời gian bất tử
    private RandomMovement parentRandomMovement;

    private void Start()
    {
        StartCoroutine(InvincibilityCoroutine());
        parentRandomMovement = GetComponentInParent<RandomMovement>();
    }

    private IEnumerator InvincibilityCoroutine()
    {
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (character != null)
        {
            if (!isInvincible)
            {
                VfxManager.instance.AlienDestroyVfx(transform.position, transform.rotation);
                AudioManager.instance.PlaySFX("Alien-Destroy");
                Destroy(parentRandomMovement.gameObject);
            }
                
        }
    }
}
