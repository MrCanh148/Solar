using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingUI : FastSingleton<SettingUI>
{
    [Header("Bt0: Setting / Bt1: Back / Bt2: Quit")]
    [SerializeField] private Button[] bts;
    [SerializeField] private GameObject UISetting;

    private void Start()
    {
        bts[0].onClick.AddListener(SettingBtFeature);
        bts[1].onClick.AddListener(BackBtFeature);
        bts[2].onClick.AddListener(() => SceneManager.LoadScene(0));
    }

    public void SettingBtFeature()
    {
        UISetting.SetActive(true);
        Time.timeScale = 0;
    }

    public void BackBtFeature()
    {
        Time.timeScale = 1;
        UISetting.SetActive(false);
    }

    public void ResetBtFeature()
    {
        SaveManager.Instance.DeleteSaveFile();
    }

}
