using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest3 : MonoBehaviour
{
    private bool isShowed = false;

    private void Update()
    {
        if (!isShowed)
        {
            isShowed =  true;
            QuestEventManager.Instance.NotifyQuest3Completed();
        }
 
    }
}
