using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicQuestPointer : MonoBehaviour, IQuestEvent
{
    [SerializeField] private Sprite arrow, cross;
    [SerializeField] private GameObject Player, pointerPrefab;
    [SerializeField] private float R = 15;
    [SerializeField] private float borderSize = 100;
    [SerializeField] private float moveSpeed = 5f;

    private List<QuestPointer> questPointers = new List<QuestPointer>();
    private Camera mainCamera;
    private Vector3 targetPosScreenPoint, playerScreenPos, direction, pointerScreenPos;
    private bool isOffScreen;

    private Vector3 toPos, fromPos, dir;
    private float angle;

    private void Awake()
    {
        QuestEventManager.Instance.RegisterQuestEvent(this);
        mainCamera = Camera.main;
    }

    private void OnDestroy()
    {
        QuestEventManager.Instance.UnregisterQuestEvent(this);
    }

    private void Update()
    {
        foreach (var questPointer in questPointers)
        {
            targetPosScreenPoint = mainCamera.WorldToScreenPoint(questPointer.TargetPos);
            isOffScreen = targetPosScreenPoint.x <= borderSize || targetPosScreenPoint.x > Screen.width - borderSize
                || targetPosScreenPoint.y <= borderSize || targetPosScreenPoint.y > Screen.height - borderSize;

            if (isOffScreen)
            {
                RotatePointerTowardTargetPos(questPointer.PointerRectTransform, questPointer.TargetPos);
                questPointer.PointerImage.sprite = arrow;

                playerScreenPos = mainCamera.WorldToScreenPoint(Player.transform.position);
                direction = (targetPosScreenPoint - playerScreenPos).normalized;
                pointerScreenPos = playerScreenPos + direction * R;

                pointerScreenPos.x = Mathf.Clamp(pointerScreenPos.x, borderSize, Screen.width - borderSize);
                pointerScreenPos.y = Mathf.Clamp(pointerScreenPos.y, borderSize, Screen.height - borderSize);

                questPointer.PointerRectTransform.position = Vector3.Lerp(questPointer.PointerRectTransform.position, pointerScreenPos, Time.deltaTime * moveSpeed);
            }
            else
            {
                questPointer.PointerImage.sprite = cross;
                questPointer.PointerRectTransform.position = Vector3.Lerp(questPointer.PointerRectTransform.position, targetPosScreenPoint, Time.deltaTime * moveSpeed);
            }
        }
    }

    private void RotatePointerTowardTargetPos(RectTransform pointerRectTransform, Vector3 targetPos)
    {
        toPos = targetPos;
        fromPos = mainCamera.transform.position;
        fromPos.z = 0f;
        dir = (toPos - fromPos).normalized;
        angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) % 360;
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }

    public QuestPointer CreatePoint(Vector3 targetPos)
    {
        GameObject pointerObject = Instantiate(pointerPrefab, transform, false);
        QuestPointer questPointer = new QuestPointer(targetPos, pointerObject.GetComponent<RectTransform>(), pointerObject.GetComponent<Image>(), arrow, cross);
        questPointers.Add(questPointer);
        return questPointer;
    }

    public void RemoveAllPoints()
    {
        foreach (var questPointer in questPointers)
        {
            Destroy(questPointer.PointerGameObject);
        }
        questPointers.Clear();
    }

    public class QuestPointer
    {
        public Vector3 TargetPos { get; private set; }
        public RectTransform PointerRectTransform { get; private set; }
        public Image PointerImage { get; private set; }
        public GameObject PointerGameObject { get; private set; }

        public QuestPointer(Vector3 targetPos, RectTransform pointerRectTransform, Image pointerImage, Sprite arrow, Sprite cross)
        {
            TargetPos = targetPos;
            PointerRectTransform = pointerRectTransform;
            PointerImage = pointerImage;
            PointerGameObject = pointerRectTransform.gameObject;
            PointerImage.sprite = arrow;
        }
    }

    public void OnQuestEnter(Quest quest)
    {
        RemoveAllPoints();
    }
}
