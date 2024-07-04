using UnityEngine;

public class Quest1 : MonoBehaviour
{
    [SerializeField] private GameObject AlienPrefab;
    private bool isShowed = false;

    private void Update()
    {
        if (!isShowed)
        {
            isShowed = true;
            QuestEventManager.Instance.NotifyQuest1Completed();
        }
   
    }

}
