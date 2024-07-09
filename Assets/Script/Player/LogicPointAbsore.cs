using UnityEngine;

public class LogicPointAbsore : MonoBehaviour
{
    public static LogicPointAbsore instance;

    [SerializeField] private int Meteoroid = 2;
    [SerializeField] private int Asteroid = 4;
    [SerializeField] private int Planet = 12;
    [SerializeField] private int LifePlanet = 36;
    [SerializeField] private int GasGiant = 108;
    [SerializeField] private int Star = 324;
    [SerializeField] private int Neutron = 1000;
    [SerializeField] private int BlackHole = 10000;

    private void Start()
    {
        instance = this;
    }

    public void AddPoint(Character c1, Character c2)
    {
        switch (c2.characterType)
        {
            case CharacterType.Meteoroid:
                c1.rb.mass += Meteoroid;
                break;
            case CharacterType.Asteroid:
                c1.rb.mass += Asteroid;
                break;
            case CharacterType.Planet:
                c1.rb.mass += Planet;
                break;
            case CharacterType.LifePlanet:
                c1.rb.mass += LifePlanet;
                break;
            case CharacterType.GasGiant:
                c1.rb.mass += GasGiant;
                break;
            case CharacterType.Star:
                c1.rb.mass += Star;
                break;
            case CharacterType.NeutronStar:
                c1.rb.mass += Neutron;
                break;
            case CharacterType.BlackHole:
                c1.rb.mass += BlackHole;
                break;
        }
    }
}

