using System.Collections.Generic;
using UnityEngine;

public class CaptureZone : MonoBehaviour
{
    public Character owner;
    public float limitedRadius = 0.5f;
    private bool canCaptureZone = true;
    private List<Character> charactersInZone = new List<Character>();
    [SerializeField] private bool IsOrbit2 = false;
    [SerializeField] private GameObject LineOrbit1;
    [SerializeField] private GameObject LineOrbit2;

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
                    owner.satellites1.Add(character);
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

        if (owner.generalityType == GeneralityType.Asteroid || owner.generalityType == GeneralityType.BlackHole)
            LineOrbit1.SetActive(false);
        else
            LineOrbit1.SetActive(true);

        if (owner.generalityType ==  GeneralityType.Star || owner.characterType == CharacterType.GasGiant)
            LineOrbit2.SetActive(true);
        else
            LineOrbit2.SetActive(false);
    }

    private void UpdateCaptureAbility()
    {
        if (owner.generalityType == GeneralityType.Planet)
        {
            if (!IsOrbit2)
                canCaptureZone = owner.satellites1.Count < SpawnPlanets.instance.GetMaxOrbit1(owner.characterType);
            else
                canCaptureZone = owner.satellites1.Count < SpawnPlanets.instance.GetMaxOrbit2(owner.characterType);
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
        character.spinSpeed = 1f;
        character.angle = Mathf.Atan2(character.tf.position.y - owner.tf.position.y, character.tf.position.x - owner.tf.position.x);
    }

    public void ArrangeSatellites()
    {
        int satelliteCount = owner.satellites1.Count;
        float angleIncrement = 360f / satelliteCount;

        for (int i = 0; i < satelliteCount; i++)
        {
            Character satellite = owner.satellites1[i];
            float angle = i * angleIncrement;
            satellite.angle = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(satellite.angle), Mathf.Sin(satellite.angle)) * limitedRadius;
            Vector3 newPosition = owner.tf.position + offset;
        }
    }
}
