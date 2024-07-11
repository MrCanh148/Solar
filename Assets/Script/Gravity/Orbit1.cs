﻿using DG.Tweening;
using UnityEngine;

public class Orbit1 : MonoBehaviour
{
    [SerializeField] private Character owner;
    [SerializeField] private CircleCollider2D colliderOrbit1;
    [SerializeField] private GameObject LineOrbit1;
    private float orbitRadius;
    private bool canOrbit = false;
    private bool shouldEnableLine, playerRespawnDone;

    private void Awake()
    {
        orbitRadius = owner.transform.localScale.x * colliderOrbit1.radius;
        owner.radiusOrbit1 = orbitRadius;
    }

    private void Update()
    {
        canOrbit = owner.satellites1.Count < SpawnPlanets.instance.GetMaxOrbit1(owner.characterType);
        orbitRadius = owner.transform.localScale.x * colliderOrbit1.radius;
        owner.radiusOrbit1 = orbitRadius;
        if (owner.isPlayer)
        {
            playerRespawnDone = ReSpawnPlayer.Instance.RespawnDone;
        }

        LogicOnOffLine();
    }

    private void LogicOnOffLine()
    {
        if (owner.isPlayer)
            shouldEnableLine = (owner.generalityType == GeneralityType.Planet ||
                                 owner.generalityType == GeneralityType.Star) && playerRespawnDone;

        if (!owner.isPlayer)
            shouldEnableLine = (owner.generalityType == GeneralityType.Planet ||
                        owner.generalityType == GeneralityType.Star);

        LineOrbit1.SetActive(shouldEnableLine);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character target = collision.GetComponent<Character>();

        if (target != null && owner != null && canOrbit)
        {
            if (owner.isPlayer && playerRespawnDone)
            {
                if (owner.generalityType == target.generalityType + 1 && target.host == null && !target.isPlayer)
                {
                    AudioManager.instance.PlaySFX("Orbit");
                    BecomeSatellite(target);
                    owner.satellites1.Add(target);
                    ArrangeSatellites();
                }
            }

            if (!owner.isPlayer)
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
        character.spinSpeed = owner.spinSpeedOrbit1;
        character.angle = Mathf.Atan2(character.tf.position.y - owner.tf.position.y, character.tf.position.x - owner.tf.position.x);
    }

    private void ArrangeSatellites()
    {
        int satelliteCount = owner.satellites1.Count;
        float angleIncrement = 6.28f / satelliteCount;
        float angleStart = Mathf.Atan2(owner.satellites1[satelliteCount - 1].tf.position.y - owner.tf.position.y, owner.satellites1[satelliteCount - 1].tf.position.x - owner.tf.position.x);
        SortSatellites(angleStart);
        for (int i = 0; i < satelliteCount; i++)
        {
            Character satellite = owner.satellites1[i];
            float newAngle = angleStart - (i * angleIncrement);
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
        for (int i = 0; i < owner.satellites1.Count - 1; i++)
        {
            for (int j = i + 1; j < owner.satellites1.Count; j++)
            {
                double angleDifference1 = NormalizeAngle(originalAngle) - NormalizeAngle(owner.satellites1[i].angle);
                double angleDifference2 = NormalizeAngle(originalAngle) - NormalizeAngle(owner.satellites1[j].angle);
                if (angleDifference1 < 0)
                    angleDifference1 += 6.28f;

                if (angleDifference2 < 0)
                    angleDifference2 += 6.28f;

                if (angleDifference1 > angleDifference2)
                {
                    Character temp = owner.satellites1[i];
                    owner.satellites1[i] = owner.satellites1[j];
                    owner.satellites1[j] = temp;
                }
            }
        }
    }
}
