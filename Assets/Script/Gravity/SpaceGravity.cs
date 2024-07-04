
using UnityEngine;

public class SpaceGravity : MonoBehaviour
{
    [SerializeField] private float gravitationForce; //luc hut
    [SerializeField] private float attractionRange; // Pham vi hut

    public float TimeConnect, countTime;

    private void OnTriggerStay2D(Collider2D other)
    {
        Vector2 direction = (transform.position - other.transform.position).normalized;

        float distance = Vector2.Distance(transform.position, other.transform.position);
        float attractionForce = gravitationForce / distance;

        other.GetComponent<Rigidbody2D>().AddForce(direction * attractionForce);

        // ==== Time to Connect
        TimeConnect = Random.Range(0.5f, 1.5f);
        countTime += Time.deltaTime;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        countTime = 0;
        TimeConnect = 0;
    }

}
