using UnityEngine;

public class Orbit1 : MonoBehaviour
{
    [SerializeField] private Character owner;
    [SerializeField] private GameObject LineOrbit1;
    [SerializeField] private float orbitRadius = 2f;
    public bool canOrbit = false;

    private void Update()
    {
        canOrbit = owner.satellites1.Count < SpawnPlanets.instance.GetMaxOrbit1(owner.characterType);
        LogicOnOffLine();
    }

    private void LogicOnOffLine()
    {
        if (owner.generalityType == GeneralityType.Asteroid || owner.generalityType == GeneralityType.BlackHole)
            LineOrbit1.SetActive(false);
        else
            LineOrbit1.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character target = collision.GetComponent<Character>();

        if (target != null && owner != null && canOrbit)
        {
            if (owner.generalityType == target.generalityType + 1 && target.host == null && !target.isPlayer)
            {
                AudioManager.instance.PlaySFX("Orbit");
                BecomeSatellite(target);
                owner.satellites1.Add(target);
                ArrangeSatellites();
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
        int satelliteCount = owner.satellites1.Count;
        float angleIncrement = 360f / satelliteCount;

        for (int i = 0; i < satelliteCount; i++)
        {
            Character satellite = owner.satellites1[i];
            float angle = i * angleIncrement;
            satellite.angle = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(satellite.angle), Mathf.Sin(satellite.angle)) * orbitRadius;
            satellite.transform.position = owner.tf.position + offset;
        }
    }
}
