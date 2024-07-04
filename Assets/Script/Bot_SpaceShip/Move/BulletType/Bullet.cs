using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float TimeLimit = 2f;
    private float timeAppear = 0f;
    public int damage;
    [HideInInspector] public Character characterOwner;

    private void Start()
    {
        AudioManager.instance.PlaySFX("Pistol");
    }

    private void Update()
    {
        timeAppear += Time.deltaTime;
        if (timeAppear > TimeLimit)
            Destroy(gameObject);

        transform.Translate(Vector2.up * speed * Time.deltaTime);


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character target = Cache.GetCharacterCollider(collision);
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
}
