using UnityEngine;

public class Orbit2 : MonoBehaviour
{
    [SerializeField] private Character owner;
    [SerializeField] private CircleCollider2D colliderOrbit2;
    [SerializeField] private GameObject LineOrbit2;
    private float orbitRadius;
    private bool canOrbit = false;

    private void Start()
    {
        orbitRadius = owner.transform.localScale.x * colliderOrbit2.radius;
    }

    private void Update()
    {
        canOrbit = owner.satellites2.Count < SpawnPlanets.instance.GetMaxOrbit2(owner.characterType);
        orbitRadius = owner.transform.localScale.x * colliderOrbit2.radius;
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
        character.radius = orbitRadius;
        character.isCapture = true;
        character.spinSpeed = 1f;
        character.angle = Mathf.Atan2(character.tf.position.y - owner.tf.position.y, character.tf.position.x - owner.tf.position.x);
    }

    private void ArrangeSatellites()
    {
        int satelliteCount = owner.satellites2.Count;
        float angleIncrement = 6.28f / satelliteCount;
        float angleStart = Mathf.Atan2(owner.satellites2[satelliteCount - 1].tf.position.y - owner.tf.position.y, owner.satellites2[satelliteCount - 1].tf.position.x - owner.tf.position.x);
        for (int i = satelliteCount - 1; i >= 0; i--)
        {
            Character satellite = owner.satellites2[i];
            satellite.angle = angleStart + ((satelliteCount - 1) - i) * angleIncrement;
        }

    }
}

