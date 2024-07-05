using UnityEngine;
using UnityEngine.UI;

public class BasicReSpawn : MonoBehaviour
{
    [SerializeField] private Button ReSpawnAsteroid;
    [SerializeField] private Button ReSpawnSmallPlanet;
    [SerializeField] private Button ReSpawnLifePlanet;
    [SerializeField] private Button ReSpawnGasGiantPlanet;
    [SerializeField] private Button ReSpawnSmallStar;
    [SerializeField] private Button ReSpawnMediumStar;
    [SerializeField] private Button ReSpawnBigStar;
    [SerializeField] private Button ReSpawnNeutronStar;
    [SerializeField] private Button ReSpawnBlackHole;

    [SerializeField] private Player player;
    [SerializeField] private GameObject SettingUI, GamePlayUI;

    private void Start()
    {
        ReSpawnAsteroid.onClick.AddListener(() => BasicReSpawnPlayer(CharacterType.Meteoroid));
        ReSpawnSmallPlanet.onClick.AddListener(() => BasicReSpawnPlayer(CharacterType.Asteroid));
        ReSpawnLifePlanet.onClick.AddListener(() => BasicReSpawnPlayer(CharacterType.Planet));
        ReSpawnGasGiantPlanet.onClick.AddListener(() => BasicReSpawnPlayer(CharacterType.LifePlanet));
        ReSpawnSmallStar.onClick.AddListener(() => BasicReSpawnPlayer(CharacterType.GasGiant));
        ReSpawnMediumStar.onClick.AddListener(() => BasicReSpawnPlayer(CharacterType.Star));
        ReSpawnNeutronStar.onClick.AddListener(() => BasicReSpawnPlayer(CharacterType.NeutronStar));
        ReSpawnBlackHole.onClick.AddListener(() => BasicReSpawnPlayer(CharacterType.BlackHole));
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
        GamePlayUI.SetActive(true);
        Time.timeScale = 1.0f;
        SpawnPlanets.instance.AdjustSpawnRates(player.characterType);
    }
}
