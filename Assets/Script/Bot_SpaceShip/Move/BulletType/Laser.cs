using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float TimeLimit = 0.5f;
    [SerializeField] private float damage = 5;
    private float timeAppear = 0f;
    private GameObject target;
    private Transform firePoint;
    [HideInInspector] public Character characterOwner;
    private bool isShoot = false;

    private void Start()
    {
        lineRenderer.positionCount = 2;
        AudioManager.instance.PlaySFX("Laser");
    }

    private void Update()
    {
        timeAppear += Time.deltaTime;
        if (timeAppear > TimeLimit)
            Destroy(gameObject);

        DrawLaser();
    }

    private void DrawLaser()
    {
        if (target != null && firePoint != null && !isShoot)
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, target.transform.position);

            RaycastHit2D[] hits = Physics2D.RaycastAll(firePoint.position, target.transform.position - firePoint.position, Mathf.Infinity);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.CompareTag(Constant.TAG_Planet) || hit.collider.gameObject.CompareTag(Constant.TAG_Player))
                    {
                        Character targetCharacter = Cache.GetCharacterCollider(hit.collider);
                        Shield shieldTarget = hit.collider.gameObject.GetComponent<Shield>();

                        if (targetCharacter == null) continue;

                        if (targetCharacter.generalityType != GeneralityType.BlackHole)
                        {
                            if (targetCharacter != characterOwner)
                            {
                                if (targetCharacter.characterType == CharacterType.LifePlanet)
                                {
                                    if (shieldTarget.ShieldPlanet > 0)
                                    {
                                        shieldTarget.ShieldPlanet -= damage;
                                        shieldTarget.TakeDamage = true;
                                    }
                                    else
                                        targetCharacter.rb.mass -= damage;
                                }
                                else
                                {
                                    targetCharacter.rb.mass -= damage;
                                    if (targetCharacter.rb.mass < 1 || (targetCharacter.characterType == CharacterType.SmallPlanet && targetCharacter.rb.mass < 20) || (targetCharacter.characterType == CharacterType.SmallStar && targetCharacter.rb.mass < 180))
                                    {
                                        if (hit.collider.gameObject.CompareTag(Constant.TAG_Player))
                                            ReSpawnPlayer.Instance.ResPlayer();
                                        else
                                        {
                                            if (targetCharacter.host != null)
                                                targetCharacter.host.satellites.Remove(targetCharacter);

                                            if (targetCharacter.generalityType == GeneralityType.Asteroid)
                                                targetCharacter.gameObject.SetActive(false);
                                            else
                                                SpawnPlanets.instance.ActiveCharacter2(targetCharacter);
                                        }
                                    }
                                }
                            }
                        }

                        isShoot = true;
                        break;
                    }

                    if (hit.collider.gameObject.CompareTag("AirSpace1"))
                    {
                        ShootTarget enermy = hit.collider.gameObject.GetComponent<ShootTarget>();
                        if (enermy != null)
                        {
                            enermy.heart -= damage;
                            if (enermy.heart <= 0)
                            {
                                VfxManager.instance.AlienDestroyVfx(hit.transform.position, hit.transform.rotation);
                                characterOwner.Kill++;
                                Destroy(enermy.gameObject);
                            }

                        }

                        isShoot = true;
                        break;
                    }
                }
            }
        }
    }

    public void SetTarget(GameObject newTarget, Transform newFirePoint)
    {
        target = newTarget;
        firePoint = newFirePoint;
    }
}
