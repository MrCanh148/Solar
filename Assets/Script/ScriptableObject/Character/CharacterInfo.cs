using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Objects/Characters")]
public class CharacterInfo : ScriptableObject
{
    public int id;
    public CharacterType characterType;
    public string namePlanet;
    public int requiredMass;
    public Sprite sprite;
    public Character characterPrefab;
    public float collisionCoefficient;
    public Vector3 scale;
    public int spawnRate;
}
