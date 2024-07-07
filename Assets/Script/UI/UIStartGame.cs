using UnityEngine;
using UnityEngine.UI;

public class UIStartGame : MonoBehaviour
{
    [Header("0:Start / 1:InGame")]
    [SerializeField] private GameObject[] AllUI;

    [Header("0:Play / 1:Quit")]
    [SerializeField] private Button[] bts;

    private void Start()
    {
        Time.timeScale = 1f;
        AudioManager.instance.PlayMusic("Theme1");
        DisAbleAllUI();
        AllUI[0].SetActive(true);
        bts[0].onClick.AddListener(PlayBtFeature);
        bts[1].onClick.AddListener(() => Application.Quit());
    }

    public void PlayBtFeature()
    {
        AllUI[0].SetActive(false);
        AllUI[1].SetActive(true);
    }

    private void DisAbleAllUI()
    {
        foreach (GameObject go in AllUI)
        {
            go.SetActive(false);
        }
    }

}