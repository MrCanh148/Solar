using System.Collections.Generic;
using UnityEngine;

public class SpawnPlanets : FastSingleton<SpawnPlanets>
{
    public List<CharacterInfo> CharacterInfos;
    public AsteroidGroup asteroidGroupPrefab;
    [SerializeField] private Transform tfCharacterManager;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float FarFromPlayerMin, FarFromPlayerMax;
    [SerializeField] private float destroyDistance;
    [SerializeField] private Player player;

    public Camera _camera;
    float FarFromPlayerY;
    float FarFromPlayerX;
    public List<Character> lstCharacter;
    public List<AsteroidGroup> asteroidGroups;
    public List<GroupPlanet> groupPlanetsPrefab;
    public List<GroupPlanet> groupPlanets;
    public Dictionary<CharacterType, int> spawnRates = new Dictionary<CharacterType, int>();
    public Dictionary<CharacterType, int> SortSpawnRates = new Dictionary<CharacterType, int>();
    public int quantityPlanetActive;

    private void Start()
    {
        _camera = Camera.main;
        UpdateDistanceSpawn();
        AdjustSpawnRates(player.characterType);
    }


    public void OnInit()
    {
        asteroidGroups.Clear();
        lstCharacter.Clear();
        quantityPlanetActive = 0;
        for (int i = 1; i <= GameManager.instance.AmountPlanet.amountAsteroidGroup; i++)
        {
            AsteroidGroup asteroidGroup = Instantiate(asteroidGroupPrefab, tfCharacterManager);
            asteroidGroups.Add(asteroidGroup);
            asteroidGroup.transform.localPosition = SpawnerCharacter();
        }

        for (int i = 1; i <= GameManager.instance.AmountPlanet.amountPlanet; i++)  //smallplanet
        {
            if (CharacterInfos[(int)CharacterType.SmallPlanet].characterPrefab != null)
            {
                Character character = Instantiate(CharacterInfos[(int)CharacterType.SmallPlanet].characterPrefab, tfCharacterManager);
                lstCharacter.Add(character);
                ActiveCharacter2(character);
                quantityPlanetActive++;
            }
        }
        foreach (var group in groupPlanetsPrefab)
        {
            GroupPlanet groupPlanet = Instantiate(group, tfCharacterManager);
            groupPlanets.Add(groupPlanet);
            groupPlanet.gameObject.SetActive(false);
        }
        if (groupPlanets.Count > 0)
        {

            groupPlanets[0].gameObject.SetActive(true);
            groupPlanets[0].transform.localPosition = SpawnerCharacter();
        }

        GameManager.instance.ChangeGameState(GameState.InGame);
    }

    public Vector3 SpawnerCharacter()
    {
        float xPos = Random.Range(FarFromPlayerX * 1.5f, GameManager.instance.status.coefficientActiveGameObject * FarFromPlayerX);
        float yPos = Random.Range(FarFromPlayerY * 1.5f, GameManager.instance.status.coefficientActiveGameObject * FarFromPlayerY);
        xPos = RamdomValue(xPos);
        yPos = RamdomValue(yPos);
        return new Vector2(player.tf.position.x + xPos, player.tf.position.y + yPos);
    }

    public Vector3 ReSpawnerAsterrooidGroup()
    {
        float distance = FarFromPlayerX > FarFromPlayerY ? FarFromPlayerX : FarFromPlayerY;

        return player.tf.position + ((Vector3)player.mainVelocity.normalized * distance * (GameManager.instance.status.coefficientActiveGameObject + 1) / 2);
    }

    public GroupPlanet GetGroupPlanet()
    {
        GroupPlanet groupPlanet = null;
        CharacterType characterType = RandomCharacterType();
        foreach (var group in groupPlanets)
        {
            if (group.masterStar.characterType == characterType)
            {
                groupPlanet = group;
                break;
            }
        }
        return groupPlanet;
    }


    public float RamdomValue(float n)
    {
        float number = n;
        int randomValue = Random.Range(0, 2);
        if (randomValue == 0)
        {
            number = -n;
        }
        else if (randomValue == 1)
        {
            number = n;
        }
        return number;
    }

    public void DeActiveCharacter(Character character)
    {
        character.gameObject.SetActive(false);

    }

    public void ActiveCharacter(Character character, CharacterType type)
    {
        if (character.isPlayer)
        {
            if (GameManager.instance.IsGameMode(GameMode.Normal))
            {
                ReSpawnPlayer.Instance.ResPlayer();
            }
            else if (GameManager.instance.IsGameMode(GameMode.Survival))
            {
                GameManager.instance.ChangeGameState(GameState.GameOver);
            }
        }
        else
        {
            character.gameObject.SetActive(true);
            character.tf.localPosition = SpawnerCharacter();
            character.velocity = RandomInitialVelocity(2f);
        }

        if (type == CharacterType.Asteroid)
            character.rb.mass = (int)Random.Range(1, 3);
        else
            character.rb.mass = GetRequiredMass(type) + (GetRequiredMass(type + 1) - GetRequiredMass(type)) / 2;

        character.isBasicReSpawn = true;
    }

    public void ActiveCharacter2(Character character)
    {
        //character.isBasicReSpawn = true;
        character.gameObject.SetActive(true);
        character.tf.localPosition = SpawnerCharacter();

        if (!character.isPlayer)
            character.velocity = RandomInitialVelocity(2f);

        CharacterType type = RandomCharacterType();
        character.rb.mass = GetRequiredMass(type) + (GetRequiredMass(type + 1) - GetRequiredMass(type)) / 2;

        character.isBasicReSpawn = true;
    }

    public Vector2 RandomInitialVelocity(float limit)
    {
        float randomX = Random.Range(-limit, limit);
        float randomY = Random.Range(-limit, limit);
        return new Vector2(randomX, randomY);
    }

    public int GetRequiredMass(CharacterType characterType)
    {
        int mass = CharacterInfos[(int)characterType].requiredMass;
        return mass;
    }

    public string GetNamePlanet(CharacterType characterType)
    {
        string name = CharacterInfos[(int)characterType].namePlanet;
        return name;
    }


    public float GetScalePlanet(CharacterType characterType)
    {
        float scale;
        if (characterType == CharacterType.Asteroid)
        {
            scale = 0.06f;
        }
        else
            scale = CharacterInfos[(int)characterType].scale.x;

        return scale;
    }

    public void AdjustSpawnRates(CharacterType characterType)
    {
        // Thay đổi tỷ lệ xuất hiện dựa trên CharacterType của người chơi
        switch (characterType)
        {
            case CharacterType.Asteroid:
                spawnRates[CharacterType.Asteroid] = 0;
                spawnRates[CharacterType.SmallPlanet] = 100;
                spawnRates[CharacterType.LifePlanet] = 0;
                spawnRates[CharacterType.GasGiantPlanet] = 0;
                spawnRates[CharacterType.SmallStar] = 0;
                spawnRates[CharacterType.MediumStar] = 0;
                spawnRates[CharacterType.BigStar] = 0;
                spawnRates[CharacterType.NeutronStar] = 0;
                spawnRates[CharacterType.BlackHole] = 0;
                spawnRates[CharacterType.BigCrunch] = 0;
                spawnRates[CharacterType.BigBang] = 0;
                break;
            case CharacterType.SmallPlanet:
                spawnRates[CharacterType.Asteroid] = 0;
                spawnRates[CharacterType.SmallPlanet] = 100;
                spawnRates[CharacterType.LifePlanet] = 60;
                spawnRates[CharacterType.GasGiantPlanet] = 30;
                spawnRates[CharacterType.SmallStar] = 10;
                spawnRates[CharacterType.MediumStar] = 0;
                spawnRates[CharacterType.BigStar] = 0;
                spawnRates[CharacterType.NeutronStar] = 0;
                spawnRates[CharacterType.BlackHole] = 0;
                spawnRates[CharacterType.BigCrunch] = 0;
                spawnRates[CharacterType.BigBang] = 0;
                break;
            case CharacterType.LifePlanet:
                spawnRates[CharacterType.Asteroid] = 0;
                spawnRates[CharacterType.SmallPlanet] = 100;
                spawnRates[CharacterType.LifePlanet] = 75;
                spawnRates[CharacterType.GasGiantPlanet] = 50;
                spawnRates[CharacterType.SmallStar] = 30;
                spawnRates[CharacterType.MediumStar] = 15;
                spawnRates[CharacterType.BigStar] = 5;
                spawnRates[CharacterType.NeutronStar] = 0;
                spawnRates[CharacterType.BlackHole] = 0;
                spawnRates[CharacterType.BigCrunch] = 0;
                spawnRates[CharacterType.BigBang] = 0;
                break;
            case CharacterType.GasGiantPlanet:
                spawnRates[CharacterType.Asteroid] = 0;
                spawnRates[CharacterType.SmallPlanet] = 100;
                spawnRates[CharacterType.LifePlanet] = 90;
                spawnRates[CharacterType.GasGiantPlanet] = 75;
                spawnRates[CharacterType.SmallStar] = 50;
                spawnRates[CharacterType.MediumStar] = 30;
                spawnRates[CharacterType.BigStar] = 15;
                spawnRates[CharacterType.NeutronStar] = 5;
                spawnRates[CharacterType.BlackHole] = 0;
                spawnRates[CharacterType.BigCrunch] = 0;
                spawnRates[CharacterType.BigBang] = 0;
                break;
            case CharacterType.SmallStar:
                spawnRates[CharacterType.Asteroid] = 0;
                spawnRates[CharacterType.SmallPlanet] = 100;
                spawnRates[CharacterType.LifePlanet] = 90;
                spawnRates[CharacterType.GasGiantPlanet] = 75;
                spawnRates[CharacterType.SmallStar] = 55;
                spawnRates[CharacterType.MediumStar] = 40;
                spawnRates[CharacterType.BigStar] = 25;
                spawnRates[CharacterType.NeutronStar] = 10;
                spawnRates[CharacterType.BlackHole] = 0;
                spawnRates[CharacterType.BigCrunch] = 0;
                spawnRates[CharacterType.BigBang] = 0;
                break;
            case CharacterType.MediumStar:
                spawnRates[CharacterType.Asteroid] = 0;
                spawnRates[CharacterType.SmallPlanet] = 100;
                spawnRates[CharacterType.LifePlanet] = 90;
                spawnRates[CharacterType.GasGiantPlanet] = 80;
                spawnRates[CharacterType.SmallStar] = 65;
                spawnRates[CharacterType.MediumStar] = 55;
                spawnRates[CharacterType.BigStar] = 35;
                spawnRates[CharacterType.NeutronStar] = 15;
                spawnRates[CharacterType.BlackHole] = 0;
                spawnRates[CharacterType.BigCrunch] = 0;
                spawnRates[CharacterType.BigBang] = 0;
                break;
            case CharacterType.BigStar:
                spawnRates[CharacterType.Asteroid] = 0;
                spawnRates[CharacterType.SmallPlanet] = 100;
                spawnRates[CharacterType.LifePlanet] = 90;
                spawnRates[CharacterType.GasGiantPlanet] = 80;
                spawnRates[CharacterType.SmallStar] = 70;
                spawnRates[CharacterType.MediumStar] = 60;
                spawnRates[CharacterType.BigStar] = 40;
                spawnRates[CharacterType.NeutronStar] = 20;
                spawnRates[CharacterType.BlackHole] = 0;
                spawnRates[CharacterType.BigCrunch] = 0;
                spawnRates[CharacterType.BigBang] = 0;
                break;
            case CharacterType.NeutronStar:
                spawnRates[CharacterType.Asteroid] = 0;
                spawnRates[CharacterType.SmallPlanet] = 100;
                spawnRates[CharacterType.LifePlanet] = 90;
                spawnRates[CharacterType.GasGiantPlanet] = 80;
                spawnRates[CharacterType.SmallStar] = 70;
                spawnRates[CharacterType.MediumStar] = 60;
                spawnRates[CharacterType.BigStar] = 40;
                spawnRates[CharacterType.NeutronStar] = 20;
                spawnRates[CharacterType.BlackHole] = 0;
                spawnRates[CharacterType.BigCrunch] = 0;
                spawnRates[CharacterType.BigBang] = 0;
                break;
            case CharacterType.BlackHole:
                spawnRates[CharacterType.Asteroid] = 0;
                spawnRates[CharacterType.SmallPlanet] = 100;
                spawnRates[CharacterType.LifePlanet] = 90;
                spawnRates[CharacterType.GasGiantPlanet] = 80;
                spawnRates[CharacterType.SmallStar] = 70;
                spawnRates[CharacterType.MediumStar] = 60;
                spawnRates[CharacterType.BigStar] = 40;
                spawnRates[CharacterType.NeutronStar] = 20;
                spawnRates[CharacterType.BlackHole] = 0;
                spawnRates[CharacterType.BigCrunch] = 0;
                spawnRates[CharacterType.BigBang] = 0;
                break;
            case CharacterType.BigCrunch:
                spawnRates[CharacterType.Asteroid] = 0;
                spawnRates[CharacterType.SmallPlanet] = 100;
                spawnRates[CharacterType.LifePlanet] = 90;
                spawnRates[CharacterType.GasGiantPlanet] = 80;
                spawnRates[CharacterType.SmallStar] = 70;
                spawnRates[CharacterType.MediumStar] = 60;
                spawnRates[CharacterType.BigStar] = 40;
                spawnRates[CharacterType.NeutronStar] = 20;
                spawnRates[CharacterType.BlackHole] = 0;
                spawnRates[CharacterType.BigCrunch] = 0;
                spawnRates[CharacterType.BigBang] = 0;
                break;
            case CharacterType.BigBang:
                spawnRates[CharacterType.Asteroid] = 0;
                spawnRates[CharacterType.SmallPlanet] = 100;
                spawnRates[CharacterType.LifePlanet] = 90;
                spawnRates[CharacterType.GasGiantPlanet] = 80;
                spawnRates[CharacterType.SmallStar] = 70;
                spawnRates[CharacterType.MediumStar] = 60;
                spawnRates[CharacterType.BigStar] = 40;
                spawnRates[CharacterType.NeutronStar] = 20;
                spawnRates[CharacterType.BlackHole] = 0;
                spawnRates[CharacterType.BigCrunch] = 0;
                spawnRates[CharacterType.BigBang] = 0;
                break;
            default:
                break;
        }
        UpGradeRate();
    }

    public void UpGradeRate()
    {
        SortSpawnRates.Clear();
        // Tạo một danh sách các cặp key-value
        var spawnRatesList = new List<KeyValuePair<CharacterType, int>>(spawnRates);

        // Sắp xếp danh sách theo giá trị value tăng dần
        spawnRatesList.Sort((x, y) => x.Value.CompareTo(y.Value));

        // Thêm các cặp key-value đã sắp xếp vào SortSpawnRates
        foreach (var pair in spawnRatesList)
        {
            SortSpawnRates[pair.Key] = pair.Value;
        }
    }

    public CharacterType RandomCharacterType()
    {
        CharacterType characterType = CharacterType.Asteroid;
        int random = Random.Range(1, 101);
        foreach (var rate in SortSpawnRates)
        {
            if (random <= rate.Value)
            {
                characterType = rate.Key;
                break;
            }
        }
        return characterType;
    }

    public void SpawnPlanetWhenCapture()
    {
        if (quantityPlanetActive < GameManager.instance.AmountPlanet.amountPlanet)
        {
            bool available = false;
            foreach (var c in lstCharacter)
            {
                if (!c.gameObject.activeSelf)
                {
                    ActiveCharacter2(c);
                    quantityPlanetActive++;
                    available = true;
                    break;
                }
            }
            if (available)
            {
                return;
            }
            else
            {
                Character character = Instantiate(CharacterInfos[(int)CharacterType.SmallPlanet].characterPrefab, tfCharacterManager);
                lstCharacter.Add(character);
                ActiveCharacter2(character);
                quantityPlanetActive++;
            }
        }
    }

    public void UpdateDistanceSpawn()
    {
        FarFromPlayerY = _camera.orthographicSize;
        FarFromPlayerX = FarFromPlayerY * (float)_camera.pixelWidth / _camera.pixelHeight;
    }

}
