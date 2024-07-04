using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CaptureZone : MonoBehaviour
{
    public Character owner;
    public float limitedRadius = 0.5f;
    private bool canCaptureZone = true;
    private List<Character> charactersInZone = new List<Character>();
    private Dictionary<Character, float> characterTimers = new Dictionary<Character, float>();

    private void Start()
    {
        // Ensure the lists are clear at the start
        charactersInZone.Clear();
        characterTimers.Clear();
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
                    characterTimers[character] = 0f;
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
            characterTimers.Remove(character);
        }
    }

    private void Update()
    {
        if (charactersInZone.Count > 0)
        {
            for (int i = charactersInZone.Count - 1; i >= 0; i--)
            {
                Character character = charactersInZone[i];
                characterTimers[character] += Time.deltaTime;

                if (characterTimers[character] > GameManager.instance.status.timeToCapture)
                {
                    AudioManager.instance.PlaySFX("Orbit");

                    if (character.host == null && !character.isPlayer)
                    {
                        BecomeSatellite(character);
                        owner.satellites.Add(character);
                        charactersInZone.RemoveAt(i);
                        characterTimers.Remove(character);

                        if (character.characterType > CharacterType.Asteroid)
                        {
                            SpawnPlanets.instance.quantityPlanetActive -= 1;
                            SpawnPlanets.instance.SpawnPlanetWhenCapture();
                        }
                    }
                }
            }
        }

        UpdateCaptureAbility();
    }

    private void UpdateCaptureAbility()
    {
        if (owner.generalityType == GeneralityType.Star)
        {
            canCaptureZone = owner.NunmberOrbit < owner.MaxOrbit;
        }
        else if (owner.generalityType == GeneralityType.BlackHole)
        {
            canCaptureZone = false;
        }
    }

    public void BecomeSatellite(Character character)
    {
        //character.tf.SetParent(owner.tf);
        character.host = owner;
        SetSatellite(character);
    }

    public void SetSatellite(Character character)
    {
        float distance = (character.tf.position - owner.tf.position).magnitude;
        character.radius = distance;
        character.isCapture = true;
        DOTween.To(() => character.radius, x => character.radius = x, SetRadius(character), 0.3f).Play();
        character.spinSpeed = RandomSpinSpeed(Random.Range(0.5f, 1.5f));
        character.angle = Mathf.Atan2(character.tf.position.y - owner.tf.position.y, character.tf.position.x - owner.tf.position.x);
    }

    public float SetRadius(Character character)
    {
        if (owner.generalityType == GeneralityType.Planet)
        {
            limitedRadius = GameManager.instance.status.coefficientRadiusPlanet * owner.circleCollider2D.radius * SpawnPlanets.instance.GetScalePlanet(owner.characterType);
        }
        else if (owner.generalityType == GeneralityType.Star)
        {
            limitedRadius = GameManager.instance.status.coefficientRadiusStar * owner.circleCollider2D.radius * SpawnPlanets.instance.GetScalePlanet(owner.characterType);
        }

        float radius = limitedRadius + owner.satellites.Count * owner.circleCollider2D.radius * SpawnPlanets.instance.GetScalePlanet(owner.characterType) * 2;
        //float radius = limitedRadius + owner.satellites.Count * (character.circleCollider2D.radius * SpawnPlanets.instance.GetScalePlanet(character.characterType) * GameManager.instance.status.coefficientDistanceCharacter);
        return radius;
    }

    public float RandomSpinSpeed(float n)
    {
        int randomValue = Random.Range(0, 2);
        return randomValue == 0 ? -n : n;
    }
}
