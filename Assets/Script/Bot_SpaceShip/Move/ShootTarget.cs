using UnityEngine;
using System.Collections.Generic;

public class ShootTarget : MonoBehaviour
{
    [Header("0-Bullet === 1-Laser === 2-Missile")]
    [SerializeField] private GameObject[] bulletPrefab;
    [SerializeField] private GameObject childObject;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireAngle = 20f; // Góc phía trước

    [SerializeField] private float bulletFireInterval = 0.1f; // Thời gian bắn đạn liên tiếp
    [SerializeField] private float laserFireInterval = 2f; // Thời gian bắn laser
    [SerializeField] private float missileFireInterval = 3f; // Thời gian phóng tên lửa

    public float heart; // Máu airspace
    private float nextFireTime = 0f;
    public Character hostAlien;
    private List<GameObject> ignoredTargets = new List<GameObject>();
    private RandomMovement moveRandom;
    private SpriteRenderer SpriteRenderer;
    private Collider2D childCollider;

    private void Start()
    {
        moveRandom = GetComponent<RandomMovement>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        childCollider = childObject.GetComponent<Collider2D>();
        if (childCollider != null)
        {
            childCollider.gameObject.AddComponent<ChildTriggerHandler>().parentHandler = this;
        }
    }

    public void SetIgnoredTargets(List<GameObject> targets, Character HostBase)
    {
        ignoredTargets = targets;
        hostAlien = HostBase;
    }

    private void Update()
    {
        if (hostAlien == null || !hostAlien.gameObject.activeSelf || hostAlien.characterType != CharacterType.LifePlanet)
        {
            hostAlien = null;
            SpriteRenderer.color = Color.white;
        }
        else if (hostAlien.isPlayer || (hostAlien.host != null && hostAlien.host.isPlayer))
            SpriteRenderer.color = Color.green;
        else
            SpriteRenderer.color = Color.red;
    }

    public void OnChildTriggerEnter(Collider2D other)
    {
        if (Time.time >= nextFireTime)
        {
            switch (moveRandom.type)
            {
                case RandomMovement.AirSpaceType.Fighter:
                    Shoot(0, bulletFireInterval, other);
                    break;

                case RandomMovement.AirSpaceType.Cruiser:
                    Shoot(1, laserFireInterval, other);
                    break;

                case RandomMovement.AirSpaceType.MissileBoat:
                    Shoot(2, missileFireInterval, other);
                    break;
            }
        }
    }

    private void Shoot(int bulletIndex, float fireInterval, Collider2D hit)
    {
        if (ignoredTargets.Contains(hit.gameObject)) return;

        Character target = hit.gameObject.GetComponent<Character>();
        ShootTarget characterTarget = hit.gameObject.GetComponent<ShootTarget>();
        if (hit.gameObject.CompareTag("AirSpace1") && characterTarget.hostAlien != null && hostAlien != null && characterTarget.hostAlien.myFamily == hostAlien.myFamily) return;

        if (target != null && (target.generalityType != GeneralityType.BlackHole) || hit.gameObject.CompareTag("AirSpace1"))
        {
            if (target != null && hostAlien != null && (target.myFamily == hostAlien.myFamily)) return;

            Vector2 directionToTarget = hit.transform.position - transform.position;
            float angle = Vector2.Angle(transform.up, directionToTarget);

            if (angle <= fireAngle / 2)
            {
                GameObject a = Instantiate(bulletPrefab[bulletIndex], firePoint.position, firePoint.rotation);

                // Gắn các thuộc tính cần thiết cho các loại đạn
                Bullet bullet = a.GetComponent<Bullet>();
                if (bullet != null)
                    bullet.characterOwner = hostAlien;

                Missile missile = a.GetComponent<Missile>();
                if (missile != null)
                {
                    missile.SetTarget(hit.gameObject);
                    missile.characterOwner = hostAlien;
                }

                Laser laser = a.GetComponent<Laser>();
                if (laser != null)
                {
                    laser.SetTarget(hit.gameObject, firePoint);
                    laser.characterOwner = hostAlien;
                }

                nextFireTime = Time.time + fireInterval;
            }
        }
    }
}
