using System.Collections.Generic;
using UnityEngine;

public class PlanetaryDefenceSystems : MonoBehaviour
{
    [SerializeField] Character owner;
    [SerializeField] List<Turret> turrets;
    [SerializeField] AntiOrbitalCannon AOC;
    [SerializeField] AntiOrbitalMissile AOM;

    [Header("AOM")]
    [SerializeField] MissileDef missileDefPrefab;
    [SerializeField] Transform firePoint;
    float timeCoolDownMissile;
    int quantityMissile;
    public List<MissileDef> missiles;
    bool isAOM;
    GameObject targetMissile;

    [Header("AOC")]
    public AntiOrbitalCannon antiOrbitalCannon;
    [SerializeField] bool isAOC;
    GameObject targetAOC;
    public float timeAttackAOC;
    public float damageAOC = 5;

    int currentKill;
    void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        currentKill = -1;
        //Missile
        timeCoolDownMissile = 0;
        quantityMissile = 1;
        isAOM = false;
        for (int i = 0; i < quantityMissile; i++)
        {
            MissileDef missile = Instantiate(missileDefPrefab, firePoint.position, firePoint.rotation);
            missiles.Add(missile);
            missile.gameObject.SetActive(false);
            missile.source = this;
            missile.characterOwner = owner;
        }
    }

    void Update()
    {
        OnChangeKill(owner.Kill);
        if (targetAOC != null)
        {
            timeAttackAOC += Time.deltaTime;
        }

        transform.rotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        if (isAOM)
        {
            timeCoolDownMissile += Time.deltaTime;

        }
        if (!owner.EvolutionDone)
        {
            currentKill = -1;
        }

        if (owner.characterType != CharacterType.LifePlanet)
            isAOC = false;
    }

    public void OnChangeKill(int newKill)
    {
        if (currentKill != newKill)
        {
            if (owner.EvolutionDone)
            {
                if (newKill < 0)
                {
                    UpdateQuantityTurret(0);
                    AOM.gameObject.SetActive(false);
                    AOC.gameObject.SetActive(false);
                }
                else if (newKill < 6)
                {
                    UpdateQuantityTurret(1);
                    AOM.gameObject.SetActive(false);
                    AOC.gameObject.SetActive(false);
                }
                else if (newKill < 12)
                {
                    UpdateQuantityTurret(2);
                    AOM.gameObject.SetActive(false);
                    AOC.gameObject.SetActive(false);
                }
                else if (newKill < 24)
                {
                    UpdateQuantityTurret(2);
                    AOM.gameObject.SetActive(true);
                    AOC.gameObject.SetActive(false);
                }
                else if (newKill < 36)
                {
                    UpdateQuantityTurret(3);
                    AOM.gameObject.SetActive(true);
                    AOC.gameObject.SetActive(false);
                }
                else if (newKill >= 36)
                {
                    UpdateQuantityTurret(4);
                    AOM.gameObject.SetActive(true);
                    AOC.gameObject.SetActive(true);
                }
            }
            else
            {
                UpdateQuantityTurret(0);
                AOM.gameObject.SetActive(false);
                AOC.gameObject.SetActive(false);

            }
            currentKill = newKill;
        }
    }

    public void UpdateQuantityTurret(int quantity)
    {
        for (int i = 0; i < turrets.Count; i++)
        {
            turrets[i].OwnerCharacter = owner;
            if (i < quantity)
            {
                turrets[i].gameObject.SetActive(true);
            }
            else
            {
                turrets[i].gameObject.SetActive(false);
            }
        }
    }

    public void ShotMissile(GameObject target)
    {
        if (missiles.Count > 0 && targetMissile != null)
        {
            missiles[0].gameObject.SetActive(true);
            missiles[0].SetTarget(target);
            missiles[0].characterOwner = this.owner;
            missiles[0].transform.position = firePoint.transform.position;
            missiles.Remove(missiles[0]);
            AudioManager.instance.PlaySFX("Laser");
        }
    }

    public void AvticeAOC(GameObject target)
    {
        antiOrbitalCannon.OwnerCharacter = owner;
        antiOrbitalCannon.targetObject = target;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        ShootTarget shootTarget = Cache.GetShootTargetCollider(collision);
        if (shootTarget != null)
        {
            if ((shootTarget.hostAlien != null && shootTarget.hostAlien.myFamily != owner.myFamily) || shootTarget.hostAlien == null)
            {
                if (timeCoolDownMissile > 3f)
                {
                    targetMissile = shootTarget.gameObject;
                    ShotMissile(targetMissile);
                    timeCoolDownMissile = 0;
                }
                if (targetAOC == null && isAOC)
                {
                    timeAttackAOC = 0;
                    targetAOC = shootTarget.gameObject;
                    AvticeAOC(targetAOC);
                    //antiOrbitalCannon.VFXPlay();
                    if (timeAttackAOC > 0.5f)
                    {
                        shootTarget.heart -= damageAOC;
                        if (shootTarget.heart <= 0)
                        {
                            owner.Kill++;
                            shootTarget.gameObject.SetActive(false);
                        }
                        timeAttackAOC = 0;
                    }
                }
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ShootTarget shootTarget = Cache.GetShootTargetCollider(collision);
        if (shootTarget != null)
        {
            if (targetMissile == shootTarget.gameObject)
            {
                targetMissile = null;
                ShotMissile(targetMissile);
            }
            if (targetAOC == shootTarget.gameObject || owner.characterType != CharacterType.LifePlanet || targetAOC == null)
            {
                targetAOC = null;
                AvticeAOC(targetAOC);
                //antiOrbitalCannon.VFXStop();
            }
        }

    }
}
