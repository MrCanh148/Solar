
using UnityEngine;

public interface IVfx
{
    public void AlienDestroyVfx(Vector3 position, Quaternion rotation);
    public void PlanetDestroyVfx(Vector3 position, Quaternion rotation);
    public void PlanetHitVfx(Vector3 position, Quaternion rotation);
}