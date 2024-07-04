using DG.Tweening;
using UnityEngine;

public class UpdateStatusCharacter : MonoBehaviour
{
    public Character owner;
    private int currentMass;
    [SerializeField] private GameObject Minimap;
    private SpriteRenderer spriteRenderer;
    private int requiredMass;
    private int currentGenerateType;

    private void Start()
    {
        EvolutionCharacter(owner);
        if (Minimap != null)
            spriteRenderer = Minimap.GetComponent<SpriteRenderer>();
        requiredMass = SpawnPlanets.instance.GetRequiredMass(owner.characterType);
        currentGenerateType = (int)owner.generalityType;
    }

    private void Update()
    {
        OnChangeMass((int)owner.rb.mass);
        OnChangeGenerateType((int)owner.generalityType);
    }

    public void UpdateInfoCharacter(Character character)
    {
        bool typeChanged;
        do
        {
            typeChanged = false;

            if (character.characterType == CharacterType.Asteroid)
                character.tf.DOScale(new Vector3(0.05f, 0.05f, 0.05f) + 0.0015f * new Vector3(character.rb.mass, character.rb.mass, character.rb.mass), 0f);

            foreach (var c in SpawnPlanets.instance.CharacterInfos)
            {
                if (character.characterType == c.characterType - 1) // Tăng CharacterType
                {
                    if (character.rb.mass >= c.requiredMass)
                    {
                        character.characterType = c.characterType;
                        character.spriteRenderer.sprite = c.sprite;

                        if (Minimap != null)
                            spriteRenderer.sprite = character.spriteRenderer.sprite;

                        character.tf.DOScale(c.scale, 0f);
                        typeChanged = true;

                        if (character.isPlayer && GameManager.instance.IsGameMode(GameMode.Normal))
                        {
                            SpawnPlanets.instance.AdjustSpawnRates(character.characterType);
                            SpawnPlanets.instance.UpdateDistanceSpawn();
                        }
                        break;
                    }
                }

                if (character.characterType == c.characterType + 1) // Giảm CharacterType
                {
                    if (character.rb.mass < requiredMass)
                    {
                        character.characterType = c.characterType;
                        character.spriteRenderer.sprite = c.sprite;

                        if (Minimap != null)
                            spriteRenderer.sprite = character.spriteRenderer.sprite;

                        character.tf.DOScale(c.scale, 0f);
                        typeChanged = true;

                        if (character.isPlayer)
                        {
                            if (GameManager.instance.IsGameMode(GameMode.Normal))
                            {
                                SpawnPlanets.instance.UpdateDistanceSpawn();
                                SpawnPlanets.instance.AdjustSpawnRates(character.characterType);
                            }
                            else if (GameManager.instance.IsGameMode(GameMode.Survival))
                            {
                                SpawnPlanets.instance.AdjustSpawnRates(character.characterType);
                                GameManager.instance.ChangeGameState(GameState.GameOver);
                            }

                        }
                        break;
                    }
                }

                if (character.characterType == c.characterType)
                {
                    requiredMass = c.requiredMass;
                }
            }
        } while (typeChanged); // Tiếp tục vòng lặp nếu loại đã thay đổi

    }


    public void EvolutionCharacter(Character character)
    {
        if (character.characterType == CharacterType.Asteroid)
        {
            character.generalityType = GeneralityType.Asteroid;
        }
        else if (character.characterType == CharacterType.SmallPlanet || character.characterType == CharacterType.LifePlanet || character.characterType == CharacterType.GasGiantPlanet)
        {
            character.generalityType = GeneralityType.Planet;
        }
        else if (character.characterType == CharacterType.SmallStar || character.characterType == CharacterType.MediumStar || character.characterType == CharacterType.NeutronStar || character.characterType == CharacterType.BigStar)
        {
            character.generalityType = GeneralityType.Star;
        }
        else if (character.characterType == CharacterType.BlackHole || character.characterType == CharacterType.BigCrunch || character.characterType == CharacterType.BigBang)
        {
            character.generalityType = GeneralityType.BlackHole;
        }
    }

    public void OnChangeMass(int newMass)
    {
        if (currentMass != newMass)
        {
            UpdateInfoCharacter(owner);
            EvolutionCharacter(owner);
            if (owner.isPlayer)
                LogicUIPlayer.Instance.UpdateInfo();
        }

        currentMass = newMass;
    }

    public void OnChangeGenerateType(int newType)
    {
        if (currentGenerateType > newType)  // tụt cấp generalityType
        {
            if (!owner.isBasicReSpawn)
            {
                owner.SoundAndVfxDie();
                owner.AllWhenDie();
                if (owner.isPlayer)
                    Debug.Log(owner.characterType);
                SpawnPlanets.instance.ActiveCharacter(owner, owner.characterType + 1);
                owner.isBasicReSpawn = false;

            }
            else
            {
                owner.AllWhenDie();
                owner.isBasicReSpawn = false;
            }

        }
        else if (currentGenerateType < newType)  // lên cấp generalityType
        {
            if (!owner.isBasicReSpawn)
            {
                owner.AllWhenDie();
                if (owner.isPlayer)
                    Debug.Log(owner.characterType);
                owner.rb.mass = SpawnPlanets.instance.GetRequiredMass(owner.characterType) + (SpawnPlanets.instance.GetRequiredMass(owner.characterType + 1) - SpawnPlanets.instance.GetRequiredMass(owner.characterType)) / 2;
                owner.isBasicReSpawn = false;
            }
            else
            {
                owner.AllWhenDie();
                owner.isBasicReSpawn = false;
            }

        }
        currentGenerateType = newType;
    }
}