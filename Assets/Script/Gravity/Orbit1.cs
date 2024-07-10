using DG.Tweening;
using UnityEngine;

public class Orbit1 : MonoBehaviour
{
    [SerializeField] private Character owner;
    [SerializeField] private CircleCollider2D colliderOrbit1;
    [SerializeField] private GameObject LineOrbit1;
    private float orbitRadius;
    private bool canOrbit = false;
    private bool shouldEnableLine, playerRespawnDone;

    private void Start()
    {
        orbitRadius = owner.transform.localScale.x * colliderOrbit1.radius;
    }

    private void Update()
    {
        canOrbit = owner.satellites1.Count < SpawnPlanets.instance.GetMaxOrbit1(owner.characterType);
        orbitRadius = owner.transform.localScale.x * colliderOrbit1.radius;

        if (owner.isPlayer)
        {
            playerRespawnDone = ReSpawnPlayer.Instance.RespawnDone;
        }

        LogicOnOffLine();
    }

    private void LogicOnOffLine()
    {
        shouldEnableLine = (owner.generalityType == GeneralityType.Planet ||
                                 owner.generalityType == GeneralityType.Star) && playerRespawnDone;

        LineOrbit1.SetActive(shouldEnableLine);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character target = collision.GetComponent<Character>();

        if (target != null && owner != null && canOrbit && playerRespawnDone)
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
        character.radius = orbitRadius;
        character.isCapture = true;
        character.spinSpeed = 1f;
        character.angle = Mathf.Atan2(character.tf.position.y - owner.tf.position.y, character.tf.position.x - owner.tf.position.x);
    }

    private void ArrangeSatellites()
    {
        int satelliteCount = owner.satellites1.Count;
        float angleIncrement = 6.28f / satelliteCount;
        float angleStart = Mathf.Atan2(owner.satellites1[satelliteCount - 1].tf.position.y - owner.tf.position.y, owner.satellites1[satelliteCount - 1].tf.position.x - owner.tf.position.x);
        for (int i = satelliteCount - 1; i >= 0; i--)
        {
            Character satellite = owner.satellites1[i];
            float newAngle = angleStart + ((satelliteCount - 1) - i) * angleIncrement;
            satellite.angle = NormalizeAngle(satellite.angle);
            newAngle = NormalizeAngle(newAngle);
            //satellite.angle = newAngle;
            DOTween.To(() => satellite.angle, x => satellite.angle = x, newAngle, 1f);
        }
    }

    public float NormalizeAngle(float angle)
    {
        while (angle > 6.28f)
        {
            angle -= 6.28f;
        }
        while (angle < 0)
        {
            angle += 6.28f;
        }
        return angle;
    }
}
