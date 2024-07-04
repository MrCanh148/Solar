using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public ShootTarget target1;
    public List<BulletTurret> bullets;
    [SerializeField] Transform shotPoint;
    [SerializeField] BulletTurret bulletPrefab;
    public float timeColdown = 1;
    public Character OwnerCharacter;

    [SerializeField] private GameObject GunMiniCam;
    public Transform gun;
    public Transform baseGun;
    Vector3 correctPos;
    float time = 0;


    private void Start()
    {
        target1 = null;
        correctPos = gun.up;
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (target1 != null)
        {
            Vector3 directionGun = (target1.transform.position - baseGun.position).normalized;

            DOTween.To(() => gun.up, x => gun.up = x, directionGun, 0.5f);
            if (GunMiniCam != null)
                DOTween.To(() => GunMiniCam.transform.up, x => GunMiniCam.transform.up = x, directionGun, 0.5f);

        }
        else
        {
            DOTween.To(() => gun.up, x => gun.up = x, correctPos, 0.5f);
            if (GunMiniCam != null)
                DOTween.To(() => GunMiniCam.transform.up, x => GunMiniCam.transform.up = x, correctPos, 0.5f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ShootTarget shootTarget = collision.GetComponent<ShootTarget>();
        if (target1 == null)
        {
            if (shootTarget != null)
            {
                if (shootTarget.hostAlien != null && shootTarget.hostAlien.myFamily == OwnerCharacter.myFamily) return;
                else
                    target1 = shootTarget;
            }
            else
            {
                target1 = null;
            }
        }

        if (target1 != null && time >= timeColdown && target1.gameObject.activeSelf)
        {
            Shot(target1);
            time = 0;
        }

    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        ShootTarget shootTarget = collision.GetComponent<ShootTarget>();
        if (target1 != null)
        {
            if (target1 == shootTarget)
            {
                target1 = null;
            }
        }

    }

    public void Shot(ShootTarget tg)
    {
        if (bullets.Count >= 1)
        {
            bullets[0].gameObject.SetActive(true);
            bullets[0].transform.position = shotPoint.position;
            bullets[0].target = tg.transform;
            bullets[0].OnInit();
            bullets[0].owner = OwnerCharacter;
            bullets.Remove(bullets[0]);

        }
        else
        {
            BulletTurret bullet = Instantiate(bulletPrefab);
            bullet.transform.position = shotPoint.position;
            bullet.target = tg.transform;
            bullet.source = this;
            bullet.owner = OwnerCharacter;
        }
    }
}
