using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    [Header("0:Back")]
    [SerializeField] private Button[] Bts;

    [Header("0:TaskUI - 1:StartGameUI")]
    [SerializeField] private GameObject[] UIs;

    private void Start()
    {
        Bts[0].onClick.AddListener(BackFeature);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            BackFeature();
    }


    public void BackFeature()
    {
        OffALlUI();
        UIs[1].SetActive(true);
    }

    private void OffALlUI()
    {
        foreach (GameObject go in UIs)
        {
            go.SetActive(false);
        }
    }
}
