using UnityEngine;

public class LogicMassDamage : MonoBehaviour
{
    public static LogicMassDamage instance;

    [SerializeField] private int MeteoroidDamage = 2;
    [SerializeField] private int AsteroidDamage = 4;
    [SerializeField] private int PlanetDamage = 8;
    [SerializeField] private int LifePlanetDamage = 16;
    [SerializeField] private int GasGiantDamage = 32;
    [SerializeField] private int StarDamage = 64;
    [SerializeField] private int NeutronDamage = 128;

    private void Start()
    {
        instance = this;
    }

    public void OnDamage(Character c1, Character c2)
    {
        switch (c2.characterType)
        {
            case CharacterType.Meteoroid:
                c1.rb.mass -= MeteoroidDamage;
                break;
            case CharacterType.Asteroid:
                c1.rb.mass -= AsteroidDamage;
                break;
            case CharacterType.Planet:
                c1.rb.mass -= PlanetDamage;
                break;
            case CharacterType.LifePlanet:
                c1.rb.mass -= LifePlanetDamage;
                break;
            case CharacterType.GasGiant:
                c1.rb.mass -= GasGiantDamage;
                break;
            case CharacterType.Star:
                c1.rb.mass -= StarDamage;
                break;
            case CharacterType.NeutronStar:
                c1.rb.mass -= NeutronDamage;
                break;

        }
    }
}
