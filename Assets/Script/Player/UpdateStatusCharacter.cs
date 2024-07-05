using DG.Tweening;
using UnityEngine;

public class UpdateStatusCharacter : MonoBehaviour
{
    public Character owner;
    private int currentMass;
    private int requiredMass;
    private int currentGenerateType;

    private void Start()
    {
        EvolutionCharacter(owner);
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

            foreach (var c in SpawnPlanets.instance.CharacterInfos)
            {
                if (character.characterType == c.characterType - 1) // Tăng CharacterType
                {
                    if (character.rb.mass >= c.requiredMass)
                    {
                        character.characterType = c.characterType;
                        character.spriteRenderer.sprite = c.sprite;

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
        if (character.characterType == CharacterType.Asteroid || character.characterType == CharacterType.Meteoroid)
        {
            character.generalityType = GeneralityType.Asteroid;
        }
        else if (character.characterType == CharacterType.Planet || character.characterType == CharacterType.LifePlanet || character.characterType == CharacterType.GasGiant)
        {
            character.generalityType = GeneralityType.Planet;
        }
        else if (character.characterType == CharacterType.Star || character.characterType == CharacterType.NeutronStar)
        {
            character.generalityType = GeneralityType.Star;
        }
        else if (character.characterType == CharacterType.BlackHole || character.characterType == CharacterType.BigBang)
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