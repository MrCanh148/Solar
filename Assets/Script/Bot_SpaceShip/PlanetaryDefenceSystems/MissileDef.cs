using UnityEngine;

public class MissileDef : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed = 200f; // Tốc độ quay của tên lửa để điều chỉnh hướng
    [SerializeField] private float TimeLimit = 3f;
    private float timeAppear = 0f;
    private GameObject target;
    [HideInInspector] public Character characterOwner;
    public int damage;
    public PlanetaryDefenceSystems source;

    private void Start()
    {
        AudioManager.instance.PlaySFX("Missile");
    }

    private void Update()
    {
        timeAppear += Time.deltaTime;
        if (timeAppear > TimeLimit)
        {
            this.gameObject.SetActive(false);
            source.missiles.Add(this);
        }

        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        if (target != null)
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;

            // Điều chỉnh hướng bay của tên lửa dựa trên hướng đến target
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Di chuyển tên lửa theo hướng đã điều chỉnh
            transform.Translate(Vector2.up * speed * Time.deltaTime, Space.Self);
        }
        else
            transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShootTarget shootTarget = Cache.GetShootTargetCollider(collision);
        if (shootTarget != null)
        {
            if ((shootTarget.hostAlien != null && shootTarget.hostAlien.myFamily != characterOwner.myFamily) || shootTarget.hostAlien == null)
            {
                shootTarget.heart -= damage;
                if (shootTarget.heart <= 0)
                {
                    VfxManager.instance.AlienDestroyVfx(shootTarget.transform.position, shootTarget.transform.rotation);
                    AudioManager.instance.PlaySFX("Alien-Destroy");
                    Destroy(shootTarget.gameObject);
                    characterOwner.Kill++;
                }
     
                this.gameObject.SetActive(false);
                source.missiles.Add(this);
            }
        }
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }
}
