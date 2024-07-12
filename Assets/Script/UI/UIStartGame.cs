using UnityEngine;
using UnityEngine.UI;

public class UIStartGame : MonoBehaviour
{
    [SerializeField] private GameObject OpenGameUI;
    [SerializeField] private GameObject MoreGameUI;

    [SerializeField] private Button GameModeBt;
    [SerializeField] private Button ShopBt;
    [SerializeField] private Button TaskBt;
    [SerializeField] private Button NormalBt;
    [SerializeField] private Button SurvivalBt;
    [SerializeField] private Button ContinueBt;
    [SerializeField] private Button ESCBt;

    private void Start()
    {
        Time.timeScale = 1f;
        AudioManager.instance.PlayMusic("Theme1");
        OpenGameUI.SetActive(true);

        GameModeBt.onClick.AddListener(() => MoreGameUI.SetActive(true));
        ESCBt.onClick.AddListener(() => MoreGameUI.SetActive(false));
        NormalBt.onClick.AddListener(NormalBtFeature);
        SurvivalBt.onClick.AddListener(SurvivalBtFeature);
        ContinueBt.onClick.AddListener(NormalBtFeature);
    }

    public void NormalBtFeature()
    {
        GameManager.instance.ChangeGameMode(GameMode.Normal);
        OpenGameUI.SetActive(false);
        SpawnPlanets.instance.OnInit();
    }

    public void SurvivalBtFeature()
    {
        GameManager.instance.ChangeGameMode(GameMode.Survival);
        GameManager.instance.timePlay = 1200;
        OpenGameUI.SetActive(false);
        SpawnPlanets.instance.OnInit();
    }

}