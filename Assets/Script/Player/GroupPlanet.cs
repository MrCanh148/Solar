using System.Collections.Generic;
using UnityEngine;

public class GroupPlanet : MonoBehaviour
{
    public List<Character> characterChilds = new List<Character>();
    List<CharacterType> characterTypes = new List<CharacterType>();
    [SerializeField] List<Vector3> StartPositions = new List<Vector3>();

    public Character masterStar;
    CharacterType masterStarType;

    private void Start()
    {
        masterStarType = masterStar.characterType;
        characterTypes.Clear();
        StartPositions.Clear();
        foreach (Character c in characterChilds)
        {
            characterTypes.Add(c.characterType);
            StartPositions.Add(c.tf.localPosition);
        }
        OnInit();
    }

    public void OnInit()
    {
        masterStar.satellites1.Clear();
        masterStar.satellites2.Clear();
        for (int i = 0; i < characterChilds.Count; i++)
        {
            Character c = characterChilds[i];
            if (c.characterType != characterTypes[i])
            {
                c.isBasicReSpawn = true;
            }
            else
            {
                c.isBasicReSpawn = false;
            }
            if (characterTypes[i] == CharacterType.Meteoroid)
            {
                c.rb.mass = 2;
            }
            else
                c.rb.mass = SpawnPlanets.instance.GetRequiredMass(characterTypes[i]) + (SpawnPlanets.instance.GetRequiredMass(characterTypes[i] + 1) - SpawnPlanets.instance.GetRequiredMass(characterTypes[i])) / 2;
            c.gameObject.SetActive(true);

        }

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
    }

    public float RandomSpinSpeed(float n)
    {
        int randomValue = Random.Range(0, 2);
        return randomValue == 0 ? -n : n;
    }
}
