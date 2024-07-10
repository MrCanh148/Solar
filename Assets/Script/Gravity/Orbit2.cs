using DG.Tweening;
using UnityEngine;

public class Orbit2 : MonoBehaviour
{
    [SerializeField] private Character owner;
    [SerializeField] private CircleCollider2D colliderOrbit2;
    [SerializeField] private GameObject LineOrbit2;
    private float orbitRadius;
    private bool canOrbit = false;
    private bool shouldEnableLine, playerRespawnDone;

    private void Start()
    {
        orbitRadius = owner.transform.localScale.x * colliderOrbit2.radius;
    }

    private void Update()
    {
        canOrbit = owner.satellites2.Count < SpawnPlanets.instance.GetMaxOrbit2(owner.characterType);
        orbitRadius = owner.transform.localScale.x * colliderOrbit2.radius;

        if (owner.isPlayer)
        {
            playerRespawnDone = ReSpawnPlayer.Instance.RespawnDone;
        }

        LogicOnOffLine();
    }

    private void LogicOnOffLine()
    {
        if (owner.isPlayer)
            shouldEnableLine = (owner.generalityType == GeneralityType.Star ||
                                    owner.characterType == CharacterType.GasGiant) && playerRespawnDone;

        if (!owner.isPlayer)
            shouldEnableLine = (owner.generalityType == GeneralityType.Star ||
                                    owner.characterType == CharacterType.GasGiant);

        LineOrbit2.SetActive(shouldEnableLine);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character target = collision.GetComponent<Character>();

        if (target != null && owner != null && canOrbit)
        {
            if (!owner.isPlayer)
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
            
            if (owner.isPlayer && playerRespawnDone)
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
        SortSatellites(angleStart);
        for (int i = 0; i < satelliteCount; i++)
        {
            Character satellite = owner.satellites2[i];
            float newAngle = angleStart - i * angleIncrement;
            satellite.angle = NormalizeAngle(satellite.angle);
            newAngle = NormalizeAngle(newAngle);
            float angleVariation = Mathf.Abs(satellite.angle - newAngle);
            if (angleVariation > 3.14f)
            {
                if (newAngle < satellite.angle)
                    satellite.angle -= 6.28f;
                else
                    newAngle -= 6.28f;
            }
            DOTween.To(() => satellite.angle, x => satellite.angle = x, newAngle, .5f);

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

    public void SortSatellites(float originalAngle)
    {
        for (int i = 0; i < owner.satellites2.Count - 1; i++)
        {
            for (int j = i + 1; j < owner.satellites2.Count; j++)
            {
                double angleDifference1 = NormalizeAngle(originalAngle) - NormalizeAngle(owner.satellites2[i].angle);
                double angleDifference2 = NormalizeAngle(originalAngle) - NormalizeAngle(owner.satellites2[j].angle);
                if (angleDifference1 < 0)
                    angleDifference1 += 6.28f;

                if (angleDifference2 < 0)
                    angleDifference2 += 6.28f;

                if (angleDifference1 > angleDifference2)
                {
                    Character temp = owner.satellites2[i];
                    owner.satellites2[i] = owner.satellites2[j];
                    owner.satellites2[j] = temp;
                }
            }
        }
    }
}

