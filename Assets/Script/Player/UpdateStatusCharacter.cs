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
        EvolutionCharacter();
        requiredMass = SpawnPlanets.instance.GetRequiredMass(owner.characterType);
        currentGenerateType = (int)owner.generalityType;
    }

    private void Update()
    {
        OnChangeMass((int)owner.rb.mass);
        OnChangeGenerateType((int)owner.generalityType);
    }

    public void UpdateInfoCharacter()
    {
        bool typeChanged;
        do
        {
            typeChanged = false;

            foreach (var c in SpawnPlanets.instance.CharacterInfos)
            {
                if (owner.characterType == c.characterType - 1) // Tăng CharacterType
                {
                    if (owner.rb.mass >= c.requiredMass)
                    {
                        owner.characterType = c.characterType;
                        owner.spriteRenderer.sprite = c.sprite;

                        owner.tf.DOScale(c.scale, 0f);
                        typeChanged = true;

                        if (owner.isPlayer && GameManager.instance.IsGameMode(GameMode.Normal))
                        {
                            SpawnPlanets.instance.AdjustSpawnRates(owner.characterType);
                            SpawnPlanets.instance.UpdateDistanceSpawn();
                        }
                        break;
                    }
                }

                if (owner.characterType == c.characterType + 1) // Giảm CharacterType
                {
                    if (owner.rb.mass < requiredMass)
                    {
                        owner.characterType = c.characterType;
                        owner.spriteRenderer.sprite = c.sprite;

                        owner.tf.DOScale(c.scale, 0f);
                        typeChanged = true;

                        if (owner.isPlayer)
                        {
                            if (GameManager.instance.IsGameMode(GameMode.Normal))
                            {
                                SpawnPlanets.instance.UpdateDistanceSpawn();
                                SpawnPlanets.instance.AdjustSpawnRates(owner.characterType);
                            }
                            else if (GameManager.instance.IsGameMode(GameMode.Survival))
                            {
                                SpawnPlanets.instance.AdjustSpawnRates(owner.characterType);
                                GameManager.instance.ChangeGameState(GameState.GameOver);
                            }

                        }
                        break;
                    }
                }

                if (owner.characterType == c.characterType)
                {
                    requiredMass = c.requiredMass;
                }
            }
        } while (typeChanged); // Tiếp tục vòng lặp nếu loại đã thay đổi

    }


    public void EvolutionCharacter()
    {
        if (owner.characterType == CharacterType.Asteroid || owner.characterType == CharacterType.Meteoroid)
        {
            owner.generalityType = GeneralityType.Asteroid;
        }
        else if (owner.characterType == CharacterType.Planet || owner.characterType == CharacterType.LifePlanet || owner.characterType == CharacterType.GasGiant)
        {
            owner.generalityType = GeneralityType.Planet;
        }
        else if (owner.characterType == CharacterType.Star || owner.characterType == CharacterType.NeutronStar)
        {
            owner.generalityType = GeneralityType.Star;
        }
        else if (owner.characterType == CharacterType.BlackHole || owner.characterType == CharacterType.BigBang)
        {
            owner.generalityType = GeneralityType.BlackHole;
        }
    }

    public void OnChangeMass(int newMass)
    {
        if (currentMass != newMass)
        {
            UpdateInfoCharacter();
            EvolutionCharacter();
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