using UnityEngine;

public enum GameState
{
    Menu,
    InGame,
    Pause,
    GameOver
};

public enum GameMode
{
    Normal,
    Survival
};

public class GameManager : FastSingleton<GameManager>
{
    public Status status;
    public AmountPlanet AmountPlanet;

    public GameState gameCurrentState;
    public GameMode currentGameMode;
    public float timePlay;
    [SerializeField] private GameObject UIGameOver;
    [SerializeField] private Player player;

    public void ChangeGameState(GameState gameState)
    {
        if (gameState == GameState.GameOver)
        {
            UIGameOver.SetActive(true);
            player.canWASD = false;
            player.ResetVelocity();
            player.spriteRenderer.enabled = false; 
        }
        gameCurrentState = gameState;
    }
    public bool IsGameState(GameState gameState)
    {
        if (gameCurrentState == gameState)
            return true;
        else
            return false;
    }

    public void ChangeGameMode(GameMode gameMode)
    {
        currentGameMode = gameMode;
    }
    public bool IsGameMode(GameMode gameMode)
    {
        if (currentGameMode == gameMode)
            return true;
        else
            return false;
    }

    private void Start()
    {

        Application.targetFrameRate = 60;
        timePlay = 1200f;
        ChangeGameState(GameState.Menu);
    }

    private void Update()
    {
        if (IsGameState(GameState.InGame))
        {
            timePlay -= Time.deltaTime;
            if (timePlay <= 0)
            {
                timePlay = 0;
                ChangeGameState(GameState.GameOver);
            }
        }

        if (IsGameMode(GameMode.Survival))
        {
            if (timePlay > 1200)  // <120
            {
                SpawnPlanets.instance.AdjustSpawnRates(CharacterType.Asteroid);
            }
            else if (timePlay > 1080)
            {
                SpawnPlanets.instance.AdjustSpawnRates(CharacterType.SmallPlanet);
            }
            else if (timePlay > 960)
            {
                SpawnPlanets.instance.AdjustSpawnRates(CharacterType.LifePlanet);
            }
            else if (timePlay > 840)
            {
                SpawnPlanets.instance.AdjustSpawnRates(CharacterType.GasGiantPlanet);
            }
            else if (timePlay > 720)
            {
                SpawnPlanets.instance.AdjustSpawnRates(CharacterType.SmallStar);
            }
            else if (timePlay > 600)
            {
                SpawnPlanets.instance.AdjustSpawnRates(CharacterType.MediumStar);
            }
            else if (timePlay > 480)
            {
                SpawnPlanets.instance.AdjustSpawnRates(CharacterType.BigStar);
            }
            else if (timePlay > 360)
            {
                SpawnPlanets.instance.AdjustSpawnRates(CharacterType.NeutronStar);
            }
            else if (timePlay > 240)
            {
                SpawnPlanets.instance.AdjustSpawnRates(CharacterType.BigBang);
            }
            else if (timePlay > 120)
            {
                SpawnPlanets.instance.AdjustSpawnRates(CharacterType.BigBang);
            }
        }
    }
}
