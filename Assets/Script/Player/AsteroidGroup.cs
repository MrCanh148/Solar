using System.Collections.Generic;
using UnityEngine;

public class AsteroidGroup : MonoBehaviour
{
    [SerializeField] List<Vector3> StartPositions = new List<Vector3>();
    int quantityAsteroid;

    private void Start()
    {
        quantityAsteroid = StartPositions.Count;
        OnInit();
    }

    public void OnInit()
    {
        Character[] characters = GetComponentsInChildren<Character>();
        foreach (Character c in characters)
        {
            c.tf.SetParent(null);
        }

        for (int i = 0; i < quantityAsteroid; i++)
        {
            Character asteroid = PoolingCharacter.instance.GetCharacterFromPool();
            asteroid.isSetup = false;
            SpawnPlanets.instance.ActiveCharacter(asteroid, asteroid.characterType);
            asteroid.tf.SetParent(this.transform);
            asteroid.tf.localPosition = StartPositions[i];
        }
    }
}
