using System.Collections.Generic;
using UnityEngine;

public class GroupPlanet : MonoBehaviour
{
    public List<Character> characterChilds1 = new List<Character>();
    public List<Character> characterChilds2 = new List<Character>();
    List<CharacterType> characterTypes1 = new List<CharacterType>();
    List<CharacterType> characterTypes2 = new List<CharacterType>();
    public Character masterStar;
    CharacterType masterStarType;

    private void Awake()
    {
        masterStarType = masterStar.characterType;
        characterTypes1.Clear();
        foreach (Character c in characterChilds1)
        {
            characterTypes1.Add(c.characterType);
        }
        characterTypes2.Clear();
        foreach (Character c in characterChilds2)
        {
            characterTypes2.Add(c.characterType);
        }
        OnInit();
    }

    public void OnInit()
    {
        //this.gameObject.GetComponent<GroupPlanet>().enabled = true;
        masterStar.satellites1.Clear();
        masterStar.satellites2.Clear();
        for (int i = 0; i < characterChilds1.Count; i++)
        {
            Character c = characterChilds1[i];
            masterStar.satellites1.Add(c);
            if (c.characterType != characterTypes1[i])
            {
                c.isBasicReSpawn = true;
            }
            else
            {
                c.isBasicReSpawn = false;
            }
            if (characterTypes1[i] == CharacterType.Meteoroid)
            {
                c.rb.mass = 2;
            }
            else
                c.rb.mass = SpawnPlanets.instance.GetRequiredMass(characterTypes1[i]) + (SpawnPlanets.instance.GetRequiredMass(characterTypes1[i] + 1) - SpawnPlanets.instance.GetRequiredMass(characterTypes1[i])) / 2;
            c.gameObject.SetActive(true);
            c.isCapture = true;
            c.host = masterStar;
            c.spinSpeed = masterStar.spinSpeedOrbit1;
            c.radius = masterStar.spinSpeedOrbit1;
        }
        for (int i = 0; i < characterChilds2.Count; i++)
        {
            Character c = characterChilds2[i];
            masterStar.satellites2.Add(c);
            if (c.characterType != characterTypes2[i])
            {
                c.isBasicReSpawn = true;
            }
            else
            {
                c.isBasicReSpawn = false;
            }
            if (characterTypes2[i] == CharacterType.Meteoroid)
            {
                c.rb.mass = 2;
            }
            else
                c.rb.mass = SpawnPlanets.instance.GetRequiredMass(characterTypes2[i]) + (SpawnPlanets.instance.GetRequiredMass(characterTypes2[i] + 1) - SpawnPlanets.instance.GetRequiredMass(characterTypes1[i])) / 2;
            c.gameObject.SetActive(true);
            c.isCapture = true;
            c.host = masterStar;
            c.spinSpeed = masterStar.spinSpeedOrbit2;
            c.radius = masterStar.spinSpeedOrbit2;
        }
        masterStar.isBasicReSpawn = true;
        if (masterStar.characterType != masterStarType)
        {
            masterStar.isBasicReSpawn = true;
        }
        else
        {
            masterStar.isBasicReSpawn = false;
        }
        masterStar.rb.mass = SpawnPlanets.instance.GetRequiredMass(masterStarType) + (SpawnPlanets.instance.GetRequiredMass(masterStarType + 1) - SpawnPlanets.instance.GetRequiredMass(masterStarType)) / 2;
        masterStar.gameObject.SetActive(true);
        masterStar.ResetAngleSatellites1();
        masterStar.ResetAngleSatellites2();
    }

    public float RandomSpinSpeed(float n)
    {
        int randomValue = Random.Range(0, 2);
        return randomValue == 0 ? -n : n;
    }
}
