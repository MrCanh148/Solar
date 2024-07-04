using UnityEngine;

public class BulletTurret : MonoBehaviour
{
    public Transform target;
    public Transform tf;
    public float speed = 5f;
    public Turret source;
    float timeAttack = 1f;
    public Character owner;
    public float damage = 5f;

    private void Start()
    {
        OnInit();

    }

    public void OnInit()
    {
        timeAttack = 0;
        AudioManager.instance.PlaySFX("Pistol");
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 direction = (target.transform.position - tf.position).normalized;
            tf.up = direction;

            timeAttack += Time.fixedDeltaTime;
            if (timeAttack > 1)
            {
                OnDespawn();
            }


            tf.Translate(Vector2.up * speed * Time.fixedDeltaTime);
        }
        else
            OnDespawn();
    }

    public void OnDespawn()
    {
        if (source != null)
        {
            gameObject.SetActive(false);
            source.bullets.Add(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShootTarget shootTarget = collision.GetComponent<ShootTarget>();
        if (shootTarget != null)
        {
            if ((shootTarget.hostAlien != null && shootTarget.hostAlien.myFamily != owner.myFamily) || shootTarget.hostAlien == null)
            {
                shootTarget.heart -= damage;
                if (shootTarget.heart <= 0)
                {
                    VfxManager.instance.AlienDestroyVfx(shootTarget.transform.position, shootTarget.transform.rotation);
                    AudioManager.instance.PlaySFX("Alien-Destroy");
                    Destroy(shootTarget.gameObject);
                    owner.Kill++;
                }

                gameObject.SetActive(false);
                source.bullets.Add(this);
            }
        }
    }
}
