using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingUI : FastSingleton<SettingUI>
{
    [Header("Bt0: Setting / Bt1: Back / Bt2: Quit / Bt3: Next / Bt4: Back")]
    [SerializeField] private Button[] bts;
    [SerializeField] private GameObject UISetting, MusicQualityUI, GodUI;

    private void Start()
    {
        BackBtFeature();
        bts[0].onClick.AddListener(SettingBtFeature);
        bts[1].onClick.AddListener(BackHomeBtFeature);
        bts[2].onClick.AddListener(() => SceneManager.LoadScene(0));
        bts[3].onClick.AddListener(NextBtFeature);
        bts[4].onClick.AddListener(BackBtFeature);
    }

    public void SettingBtFeature()
    {
        UISetting.SetActive(true);
        Time.timeScale = 0;
    }

    public void BackHomeBtFeature()
    {
        Time.timeScale = 1;
        UISetting.SetActive(false);
    }

    public void NextBtFeature()
    {
        MusicQualityUI.SetActive(false);
        GodUI.SetActive(true);
        bts[3].gameObject.SetActive(false);
        bts[4].gameObject.SetActive(true);
    }

    public void BackBtFeature()
    {
        MusicQualityUI.SetActive(true);
        GodUI.SetActive(false);
        bts[3].gameObject.SetActive(true);
        bts[4].gameObject.SetActive(false);
    }

    public void ResetBtFeature()
    {
        SaveManager.Instance.DeleteSaveFile();
    }

}
