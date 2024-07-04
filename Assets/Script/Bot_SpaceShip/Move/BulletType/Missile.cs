using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed = 200f; // Tốc độ quay của tên lửa để điều chỉnh hướng
    [SerializeField] private float TimeLimit = 3f;

    private float timeAppear = 0f;
    private GameObject target;
    [HideInInspector] public Character characterOwner;
    public int damage;

    private Vector2 direction;
    private float angle;
    private Quaternion targetRotation;

    private void Start()
    {
        AudioManager.instance.PlaySFX("Missile");
    }

    private void Update()
    {
        timeAppear += Time.deltaTime;
        if (timeAppear > TimeLimit)
            Destroy(gameObject);

        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        if (target != null)
        {
            direction = (target.transform.position - transform.position).normalized;

            // Điều chỉnh hướng bay của tên lửa dựa trên hướng đến target
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Di chuyển tên lửa theo hướng đã điều chỉnh
            transform.Translate(Vector2.up * speed * Time.deltaTime, Space.Self);
        }
        else
            transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character target = collision.gameObject.GetComponent<Character>();
        Shield shieldTarget = collision.gameObject.GetComponent<Shield>();

        if (target == null) return;

        if (target.generalityType == GeneralityType.Asteroid || target.generalityType == GeneralityType.Planet)
        {
            if (target != characterOwner)
            {
                if (target.characterType == CharacterType.LifePlanet)
                {
                    if (shieldTarget.ShieldPlanet > 0)
                    {
                        shieldTarget.ShieldPlanet -= damage;
                        shieldTarget.TakeDamage = true;
                    }
                    else
                        target.rb.mass -= damage;
                }
                else
                {
                    target.rb.mass -= damage;
                    if (target.rb.mass < 1 || (target.characterType == CharacterType.SmallPlanet && target.rb.mass < 20)
                                          || (target.characterType == CharacterType.SmallStar && target.rb.mass < 180))
                    {
                        VfxManager.instance.PlanetDestroyVfx(target.transform.position, target.transform.rotation);
                        if (collision.gameObject.CompareTag(Constant.TAG_Player))
                            ReSpawnPlayer.Instance.ResPlayer();

                        else
                        {
                            if (target.host != null)
                                target.host.satellites.Remove(target);

                            if (target.generalityType == GeneralityType.Asteroid)
                                target.gameObject.SetActive(false);
                            else
                                SpawnPlanets.instance.ActiveCharacter2(target);
                        }
                    }
                }
            }

            Destroy(gameObject);
        }
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }
}
