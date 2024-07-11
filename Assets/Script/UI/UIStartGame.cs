using UnityEngine;
using UnityEngine.UI;

public class UIStartGame : MonoBehaviour
{
    [Header("0:Start / 1:InGame")]
    [SerializeField] private GameObject[] AllUI;

    [Header("0:Normal / 1:Quit / 2:Survival")]
    [SerializeField] private Button[] bts;

    private void Start()
    {
        Time.timeScale = 1f;
        AudioManager.instance.PlayMusic("Theme1");
        DisAbleAllUI();
        AllUI[0].SetActive(true);
        bts[0].onClick.AddListener(NormalBtFeature);
        bts[1].onClick.AddListener(() => Application.Quit());
        bts[2].onClick.AddListener(SurvivalBtFeature);
    }

    public void NormalBtFeature()
    {
        GameManager.instance.ChangeGameMode(GameMode.Normal);
        AllUI[0].SetActive(false);
        AllUI[1].SetActive(true);
        SpawnPlanets.instance.OnInit();
    }

    public void SurvivalBtFeature()
    {
        GameManager.instance.ChangeGameMode(GameMode.Survival);
        GameManager.instance.timePlay = 1200;
        AllUI[0].SetActive(false);
        AllUI[1].SetActive(true);
        SpawnPlanets.instance.OnInit();
    }

    private void DisAbleAllUI()
    {
        foreach (GameObject go in AllUI)
        {
            go.SetActive(false);
        }
    }

}