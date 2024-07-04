using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button BackBt;
    [SerializeField] private GameObject UIGameOver, StartGameUI, GameplayUI;
    [SerializeField] private TextMeshProUGUI infoText;

    private void Start()
    {
        GameplayUI.SetActive(false);
        BackBt.onClick.AddListener(BackFeature);
        StartCoroutine(LoadText("<>\r\nOther Info \r\n_will coming soon\r\n................."));
    }

    public void BackFeature()
    {
        UIGameOver.SetActive(false);
        StartGameUI.SetActive(true);
        SceneManager.LoadScene(0);
    }

    private IEnumerator LoadText(string fullText)
    {
        infoText.text = "";
        foreach (char c in fullText)
        {
            infoText.text += c;
            yield return new WaitForSeconds(0.005f);
        }
    }
}
