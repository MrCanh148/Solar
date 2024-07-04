using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchieUI : MonoBehaviour
{
    private Achievemnet[] achieve;
    [SerializeField] private Transform achieveContain;
    [SerializeField] private GameObject AchievePrefab;
    [SerializeField] private TextMeshProUGUI InfoText;
    [SerializeField] private GameObject[] UIGame; //0: Achieve - 1: Title
    [SerializeField] private Button BackBt;

    private void Start()
    {
        achieve = Resources.LoadAll<Achievemnet>("Achievement");
        InfoAchieve();
        BackBt.onClick.AddListener(BackBtFeature);
    }

    private void InfoAchieve()
    {
        for (int i = 0; i < achieve.Length; i++)
        {
            int index = i;
            GameObject go = Instantiate(AchievePrefab, achieveContain);
            go.GetComponent<Button>().onClick.AddListener(() => OnAchieveClick(index));
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = achieve[i].Name;
            go.transform.GetChild(3).GetComponent<Image>().sprite = achieve[i].Icon;

            if (SaveManager.Instance.save.DoneAchievementFlag[i] == 0)
                go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Income";
            else
                go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Complete";
        }
    }

    private void OnAchieveClick(int i)
    {
        Achievemnet achieveInfo = achieve[i];
        InfoText.text = achieveInfo.Info;
    }

    private void BackBtFeature()
    {
        UIGame[0].SetActive(false);
        UIGame[1].SetActive(true);
    }
}
