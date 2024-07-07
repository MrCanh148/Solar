using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CaptureZone : MonoBehaviour
{
    public Character owner;
    public float limitedRadius = 0.5f;
    private bool canCaptureZone = true;
    private List<Character> charactersInZone = new List<Character>();

    private void Start()
    {
        charactersInZone.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = Cache.GetCharacterCollider(collision);

        if (canCaptureZone && character != null && owner != null)
        {
            if (owner.generalityType == character.generalityType + 1 && character.host == null && !character.isPlayer)
            {
                if (!charactersInZone.Contains(character))
                {
                    charactersInZone.Add(character);
                    AudioManager.instance.PlaySFX("Orbit");

                    BecomeSatellite(character);
                    owner.satellites.Add(character);
                    charactersInZone.Remove(character);

                    if (character.generalityType > GeneralityType.Asteroid)
                    {
                        SpawnPlanets.instance.quantityPlanetActive -= 1;
                        SpawnPlanets.instance.SpawnPlanetWhenCapture();
                    }

                    ArrangeSatellites();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Character character = Cache.GetCharacterCollider(collision);

        if (charactersInZone.Contains(character))
        {
            charactersInZone.Remove(character);
        }
    }

    private void Update()
    {
        UpdateCaptureAbility();
    }

    private void UpdateCaptureAbility()
    {
        if (owner.generalityType == GeneralityType.Planet)
        {
            canCaptureZone = owner.NumberOrbit1 < SpawnPlanets.instance.GetMaxOrbit1(owner.characterType);
        }
        else if (owner.generalityType == GeneralityType.BlackHole)
        {
            canCaptureZone = false;
        }
    }

    public void BecomeSatellite(Character character)
    {
        character.host = owner;
        SetSatellite(character);
    }

    public void SetSatellite(Character character)
    {
        float distance = (character.tf.position - owner.tf.position).magnitude;
        character.radius = distance;
        character.isCapture = true;
        character.spinSpeed = RandomSpinSpeed(1.0f);
        character.angle = Mathf.Atan2(character.tf.position.y - owner.tf.position.y, character.tf.position.x - owner.tf.position.x);
    }

    public float SetRadius()
    {
        float radius = limitedRadius;
        return radius;
    }

    public float RandomSpinSpeed(float n)
    {
        return n;
    }

    public void ArrangeSatellites()
    {
        int satelliteCount = owner.satellites.Count;
        float angleIncrement = 360f / satelliteCount;

        for (int i = 0; i < satelliteCount; i++)
        {
            Character satellite = owner.satellites[i];
            float angle = i * angleIncrement;
            satellite.angle = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(satellite.angle), Mathf.Sin(satellite.angle)) * SetRadius();
            Vector3 newPosition = owner.tf.position + offset;
        }
    }
}
