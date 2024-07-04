using UnityEngine;

public class Quest : MonoBehaviour
{
    public enum TypeQuest
    {
        Quest1,
        Quest2,
        Quest3,
        Quest4
    }

    public TypeQuest type;
    [SerializeField] private GameObject[] dashCircle;
    [SerializeField] private float speedRotate;

    private Quest1 quest1;
    private Quest2 quest2;
    private Quest3 quest3;
    private Quest4 quest4;

    private void Start()
    {
        if (type == TypeQuest.Quest1)
        {
            quest1 = GetComponent<Quest1>();
            quest1.enabled = false;
        }

        if (type == TypeQuest.Quest2)
        {
            quest2 = GetComponent<Quest2>();
            quest2.enabled = false;
        }

        if (type == TypeQuest.Quest3)
        {
            quest3 = GetComponent<Quest3>();
            quest3.enabled = false;
        }

        if (type == TypeQuest.Quest4)
        {
            quest4 = GetComponent<Quest4>();
            quest4.enabled = false;
        }
    }

    private void Update()
    {
        RotateObject(dashCircle[0], speedRotate);
        RotateObject(dashCircle[1], -speedRotate);
    }

    private void RotateObject(GameObject obj, float speed)
    {
        if (obj != null)
        {
            float currentRotation = obj.transform.eulerAngles.z;
            float newRotation = currentRotation + speed * Time.deltaTime;
            obj.transform.rotation = Quaternion.Euler(0f, 0f, newRotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (type == TypeQuest.Quest1)
            {
                quest1.enabled = true;
            }
            if (type == TypeQuest.Quest2)
            {
                quest2.enabled = true;
            }
            if (type == TypeQuest.Quest3)
            {
                quest3.enabled = true;
            }
            if (type == TypeQuest.Quest4)
            {
                quest4.enabled = true;
            }

            QuestEventManager.Instance.NotifyQuestEnter(this);      // Notify all IQuestEvent implementers

            //off 2 circle
            dashCircle[0].SetActive(false);
            dashCircle[1].SetActive(false);
        }
    }
}
