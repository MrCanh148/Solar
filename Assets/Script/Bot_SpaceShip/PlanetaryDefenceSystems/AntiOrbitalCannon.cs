using System.Collections.Generic;
using UnityEngine;

public class AntiOrbitalCannon : MonoBehaviour
{
    public GameObject laserOrigin; // GameObject A, nguồn phát laser
    public GameObject targetObject; // GameObject B, đích nhắm  
    public LineRenderer laserLineRenderer;
    public Character OwnerCharacter;
    [SerializeField] public GameObject startVFX;
    [SerializeField] public GameObject endVFX;
    List<ParticleSystem> listVFX = new List<ParticleSystem>();

    private void Start()
    {
        startVFX.SetActive(false);
        endVFX.SetActive(false);
    }

    private void Update()
    {
        ShootLaser();
    }

    private void ShootLaser()
    {
        if (targetObject != null)
        {
            // Cập nhật vị trí đầu và cuối của laser
            laserLineRenderer.SetPosition(0, laserOrigin.transform.position);
            laserLineRenderer.SetPosition(1, targetObject.transform.position);
            Vector2 direction = (Vector2)(targetObject.transform.position - laserOrigin.transform.position);
            RaycastHit2D[] hits = Physics2D.RaycastAll(laserOrigin.transform.position, direction.normalized, direction.magnitude);
            startVFX.transform.position = laserOrigin.transform.position;
            endVFX.transform.position = targetObject.transform.position;
            foreach (RaycastHit2D h in hits)
            {
                if (h.collider.CompareTag(Constant.TAG_AirSpace1))
                {
                    ShootTarget shootTarget = h.collider.GetComponent<ShootTarget>();
                    {
                        if (shootTarget.hostAlien != OwnerCharacter)
                        {
                            laserLineRenderer.SetPosition(1, h.point);
                            endVFX.transform.position = h.point;
                        }
                    }
                }
            }
            // Kích hoạt laser
            laserLineRenderer.enabled = true;

        }
        else
            laserLineRenderer.enabled = false;
    }

    public void FillListVFX()
    {
        for (int i = 0; i < startVFX.transform.childCount; i++)
        {
            var ps = startVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
            {
                listVFX.Add(ps);
            }
        }

        for (int i = 0; i < endVFX.transform.childCount; i++)
        {
            var ps = endVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
            {
                listVFX.Add(ps);
            }
        }
    }

    public void VFXPlay()
    {
        startVFX.SetActive(true);
        endVFX.SetActive(true);
    }

    public void VFXStop()
    {
        startVFX.SetActive(false);
        endVFX.SetActive(false);
    }
}
