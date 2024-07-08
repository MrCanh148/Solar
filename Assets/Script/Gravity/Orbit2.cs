using UnityEngine;

public class Orbit2 : MonoBehaviour
{
    [SerializeField] private Character owner;
    [SerializeField] private GameObject LineOrbit2;
    [SerializeField] private float orbitRadius = 2f;
    private bool canOrbit = false;

    private void Update()
    {
        canOrbit = owner.satellites2.Count < SpawnPlanets.instance.GetMaxOrbit2(owner.characterType);
        LogicOnOffLine();
    }

    private void LogicOnOffLine()
    {
        if (owner.generalityType == GeneralityType.Star || owner.characterType == CharacterType.GasGiant)
            LineOrbit2.SetActive(true);
        else
            LineOrbit2.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character target = collision.GetComponent<Character>();

        if (target != null && owner != null && canOrbit)
        {
            if (target.host == null && !target.isPlayer)
            {
                if (((owner.characterType == CharacterType.GasGiant || owner.characterType == CharacterType.Star)
                && (target.characterType == CharacterType.Planet || target.characterType == CharacterType.LifePlanet))
                || (owner.characterType == CharacterType.NeutronStar && target.generalityType == GeneralityType.Planet))
                {
                    AudioManager.instance.PlaySFX("Orbit");
                    BecomeSatellite(target);
                    owner.satellites2.Add(target);
                    ArrangeSatellites();
                }
            }
        }
    }

    private void BecomeSatellite(Character character)
    {
        character.host = owner;
        SetSatellite(character);
    }

    private void SetSatellite(Character character)
    {
        float distance = (character.tf.position - owner.tf.position).magnitude;
        character.radius = distance;
        character.isCapture = true;
        character.spinSpeed = 1f;
        character.angle = Mathf.Atan2(character.tf.position.y - owner.tf.position.y, character.tf.position.x - owner.tf.position.x);
    }

    private void ArrangeSatellites()
    {
        int satelliteCount = owner.satellites2.Count;
        float angleIncrement = 360f / satelliteCount;

        for (int i = 0; i < satelliteCount; i++)
        {
            Character satellite = owner.satellites2[i];
            float angle = i * angleIncrement;
            satellite.angle = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(satellite.angle), Mathf.Sin(satellite.angle)) * orbitRadius;
            satellite.transform.position = owner.tf.position + offset;
        }
    }
}

