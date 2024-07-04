using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIStartGame : MonoBehaviour
{
    [Header("0:GamePlay / 1:Setting / 2:Shop / 3:Task / 4:StartGame")]
    [SerializeField] private GameObject[] AllUI;

    [Header("0:GameMode / 1:Shop / 2:Task / 3:Setting / 4:Exit / 5:Normal / 6:Survival")]
    [SerializeField] private Button[] bts;

    [SerializeField] private GameObject AllInOne, Hole, OneInAll, Player, Select, Quest;
    [SerializeField] Player player;

    [SerializeField] private Animator StartAnimator;
    private SpriteRenderer spriteRenderer;
    private bool chachaboom = false;

    const string ENTER_GAME = "Start";

    private void Start()
    {
        Time.timeScale = 1f;
        AudioManager.instance.PlayMusic("Theme1");
        player.canWASD = false;
        DisAbleAllUI();
        Quest.SetActive(false);
        AllUI[4].SetActive(true);
        spriteRenderer = Player.GetComponent<SpriteRenderer>();
        StartAnimator = AllUI[4].GetComponent<Animator>();

        bts[0].onClick.AddListener(GameModeFeature);
        bts[1].onClick.AddListener(ShopFeature);
        bts[2].onClick.AddListener(TaskFeature);
        bts[3].onClick.AddListener(SettingFeature);
        bts[4].onClick.AddListener(ExitFeature);
        bts[5].onClick.AddListener(NormalFeature);
        bts[6].onClick.AddListener(SurvivalFeature);
    }

    private void Update()
    {
        if (player.rb.mass >= 1000000 && !chachaboom)
        {
            StartCoroutine(ChaChaBoomBoom());
            DisAbleAllUI();
        }
    }

    public void GameModeFeature()
    {
        Select.SetActive(true);
    }

    public void ShopFeature()
    {
        DisAbleAllUI();
        AllUI[2].SetActive(true);
    }

    public void TaskFeature()
    {
        DisAbleAllUI();
        AllUI[3].SetActive(true);
    }

    public void SettingFeature()
    {
        DisAbleAllUI();
        AllUI[1].SetActive(true);
    }

    public void ExitFeature()
    {
        Application.Quit();
    }

    private void NormalFeature()
    {
        GameManager.instance.ChangeGameMode(GameMode.Normal);
        Quest.SetActive(true);
        bts[5].interactable = false;
        StartAnimator.SetTrigger(ENTER_GAME);
        StartCoroutine(RunAnimator());
    
    }

    public void SurvivalFeature()
    {
        GameManager.instance.ChangeGameMode(GameMode.Survival);
        GameManager.instance.timePlay = 1200;
        bts[6].interactable = false;
        StartAnimator.SetTrigger(ENTER_GAME);
        StartCoroutine(RunAnimator());
    
    }

    private void DisAbleAllUI()
    {
        foreach (GameObject go in AllUI)
        {
            go.SetActive(false);
        }
    }

    private IEnumerator RunAnimator()
    {
        yield return new WaitForSeconds(1f);
        AllUI[4].SetActive(false);
        StartCoroutine(ChaChaBoomBoom());
        bts[5].interactable = true;
        bts[6].interactable = true;
    }

    private IEnumerator ChaChaBoomBoom()
    {
        chachaboom = true;
        AllInOne.SetActive(true);
        AudioManager.instance.PlaySFX("bb1");
        yield return new WaitForSeconds(2f);
        spriteRenderer.enabled = false;
        Hole.SetActive(true);
        yield return new WaitForSeconds(3f);
        AudioManager.instance.PlaySFX("bb2");
        AllInOne.SetActive(false);
        player.characterType = CharacterType.NeutronStar;
        OneInAll.SetActive(true);
        yield return new WaitForSeconds(1f);
        Hole.SetActive(false);
        player.rb.mass = 2f;
        yield return new WaitForSeconds(5f);
        player.characterType = CharacterType.Asteroid;
        spriteRenderer.enabled = true;
        player.canWASD = true;
        AllUI[0].SetActive(true);
        OneInAll.SetActive(false);
        yield return new WaitForSeconds(1f);
        SpawnPlanets.instance.OnInit();
        chachaboom = false;
    }
}