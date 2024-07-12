using System.Collections.Generic;
using UnityEngine;

public class AntiOrbitalMissile : MonoBehaviour
{
    [HideInInspector] public Character OwnerCharacter;
    public MissileDef MissileDefPrefab;
    List<MissileDef> lstMissile = new List<MissileDef>();
    float timeShoot;
    // Start is called before the first frame update
    //GameManager.instance.DPSParameters.
    void Start()
    {
        timeShoot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeShoot > 0)
        {
            timeShoot -= Time.deltaTime;
        }
    }

    public void Shoot()
    {
        Debug.Log("AOT shoot");
        timeShoot = GameManager.instance.DPSParameters.cooldownAOT;
    }

    private MissileDef GetMissile()
    {
        MissileDef missile = lstMissile.Find(m => !m.gameObject.activeSelf); // Tìm phần tử không active

        if (missile == null)
        {
            // Nếu không có phần tử nào không active, tạo mới một MissileDef
            //missile  = Instantiate(MissileDefPrefab, firePoint.position, firePoint.rotation);
            lstMissile.Add(missile); // Thêm vào danh sách
        }

        return missile;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShootTarget shootTarget = Cache.GetShootTargetCollider(collision);
        if (shootTarget != null)
        {
            if (shootTarget.hostAlien.myFamily != OwnerCharacter.myFamily)
            {
                if (timeShoot <= 0)
                {
                    Shoot();
                }
            }
        }
    }
}
