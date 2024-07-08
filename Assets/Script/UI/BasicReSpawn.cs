using UnityEngine;
using UnityEngine.UI;

public class BasicReSpawn : MonoBehaviour
{
    [Header("0:Meteo / 1:Aster / 2:Planet / 3:Life / 4:Gas / 5:Star / 6:Neu / 7:BlackHole")]
    [SerializeField] private Button[] bts;
    [SerializeField] private Player player;
    [SerializeField] private GameObject SettingUI;

    private void Start()
    {
        bts[0].onClick.AddListener(() => BasicReSpawnPlayer(CharacterType.Meteoroid));
        bts[1].onClick.AddListener(() => BasicReSpawnPlayer(CharacterType.Asteroid));
        bts[2].onClick.AddListener(() => BasicReSpawnPlayer(CharacterType.Planet));
        bts[3].onClick.AddListener(() => BasicReSpawnPlayer(CharacterType.LifePlanet));
        bts[4].onClick.AddListener(() => BasicReSpawnPlayer(CharacterType.GasGiant));
        bts[5].onClick.AddListener(() => BasicReSpawnPlayer(CharacterType.Star));
        bts[6].onClick.AddListener(() => BasicReSpawnPlayer(CharacterType.NeutronStar));
        bts[7].onClick.AddListener(() => BasicReSpawnPlayer(CharacterType.BlackHole));
    }

    public void BasicReSpawnPlayer(CharacterType characterType)
    {
        if (GameManager.instance.IsGameMode(GameMode.Normal))
        {
            player.isBasicReSpawn = true;
            if (characterType == CharacterType.Asteroid)
            {
                player.rb.mass = 2;
            }
            else
            {
                player.rb.mass = SpawnPlanets.instance.GetRequiredMass(characterType) + (SpawnPlanets.instance.GetRequiredMass(characterType + 1) - SpawnPlanets.instance.GetRequiredMass(characterType)) / 2;
            }
            ReSpawnPlayer.Instance.ResPlayer();
        }
        SettingUI.SetActive(false);
        Time.timeScale = 1.0f;
        SpawnPlanets.instance.AdjustSpawnRates(player.characterType);
    }
}
