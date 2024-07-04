using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBot : MonoBehaviour, IQuest2Listener, IQuest1Listenner, IQuest3Listenner, IQuest4Listenner
{
    private Dictionary<string, BotChatText[]> botChatTextsCache = new Dictionary<string, BotChatText[]>();

    [SerializeField] private GameObject TextDisplay, PressEnterText, BotUI;
    [SerializeField] TextMeshProUGUI ChatText;
    [SerializeField] private float TimeShowText = 0.01f;
    [SerializeField] private float TimeDelayShowGameObjectText = 1f;
    [SerializeField] private Button TaptoClose;

    private BotChatText[] BotChatText;
    private int currentIndex = 0;
    private bool isDisplayingText = false;
    private bool displayFullTextImmediately = false;
    private bool canPressEnter = false;
    private bool isInitialBotChat = true;
    private string currentFullText = "";
    private int currentTextIndex = 0;
    private bool isFade = false;

    private Coroutine displayCoroutine;

    private void Awake()
    {
        QuestEventManager.Instance.RegisterListener(this);
        QuestEventManager.Instance.Register1Listener(this);
        QuestEventManager.Instance.Register3Listener(this);
        QuestEventManager.Instance.Register4Listener(this);
    }

    private void OnDestroy()
    {
        QuestEventManager.Instance.UnregisterListener(this);
        QuestEventManager.Instance.Unregister1Listener(this);
        QuestEventManager.Instance.Unregister3Listener(this);
        QuestEventManager.Instance.Unregister4Listener(this);
    }

    private void Start()
    {
        TaptoClose.onClick.AddListener(OnReturnKeyPressed);
        StartCoroutine(GameObjectTextDisplayer());

        botChatTextsCache["BotStartGame"] = Resources.LoadAll<BotChatText>("BotChatText/BotStartGame");
        botChatTextsCache["BotSurvival"] = Resources.LoadAll<BotChatText>("BotChatText/BotSurvival");
        botChatTextsCache["BotQuest1"] = Resources.LoadAll<BotChatText>("BotChatText/BotQuest1");
        botChatTextsCache["BotQuest2"] = Resources.LoadAll<BotChatText>("BotChatText/BotQuest2");
        botChatTextsCache["BotQuest3"] = Resources.LoadAll<BotChatText>("BotChatText/BotQuest3");
        botChatTextsCache["BotQuest4"] = Resources.LoadAll<BotChatText>("BotChatText/BotQuest4");

        if (GameManager.instance.currentGameMode == GameMode.Normal)
            BotChatText = botChatTextsCache["BotStartGame"];
        else if (GameManager.instance.currentGameMode == GameMode.Survival)
            BotChatText = botChatTextsCache["BotSurvival"];

        isInitialBotChat = true;
    }

    private void OnEnable()
    {
        if (!string.IsNullOrEmpty(currentFullText))
        {
            StartDisplayTextOverTime(currentFullText, currentTextIndex);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && canPressEnter)
        {
            OnReturnKeyPressed();
        }

        if (currentIndex == 2 || !isInitialBotChat || GameManager.instance.currentGameMode == GameMode.Survival)
        {
            if (!isFade)
            {
                LogicUIPlayer.Instance.BgFadeIn(2f);
                isFade = true;
            }
        }
    }

    private IEnumerator DisplayTextOverTime(string fullText, int startIndex = 0)
    {
        AudioManager.instance.PlaySFX("Robot");
        isDisplayingText = true;
        displayFullTextImmediately = false;
        ChatText.text = "";

        currentFullText = fullText;
        currentTextIndex = startIndex;

        for (int i = startIndex; i < fullText.Length; i++)
        {
            if (displayFullTextImmediately)
            {
                ChatText.text = fullText;
                break;
            }

            ChatText.text += fullText[i];
            yield return new WaitForSecondsRealtime(TimeShowText);
        }
        canPressEnter = true;
        isDisplayingText = false;
        currentFullText = "";
        currentTextIndex = 0;
    }

    private IEnumerator GameObjectTextDisplayer()
    {
        TextDisplay.SetActive(false);
        yield return new WaitForSecondsRealtime(TimeDelayShowGameObjectText);
        TextDisplay.SetActive(true);
        PressEnterText.SetActive(true);

        if (BotChatText.Length > 0)
        {
            StartDisplayTextOverTime(BotChatText[currentIndex].text);
        }
    }

    private void StartDisplayTextOverTime(string text, int startIndex = 0)
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }
        displayCoroutine = StartCoroutine(DisplayTextOverTime(text, startIndex));
    }

    private void OnReturnKeyPressed()
    {
        if (isDisplayingText)
        {
            displayFullTextImmediately = true;
        }
        else
        {
            if (isInitialBotChat)
            {
                if (currentIndex < BotChatText.Length - 1)
                {
                    currentIndex++;
                    StartDisplayTextOverTime(BotChatText[currentIndex].text);
                }
                else if (currentIndex == BotChatText.Length - 1)
                {
                    BotUI.SetActive(false);
                    gameObject.SetActive(false);
                }
            }
            else
            {
                BotUI.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }

    private void HandleQuestEvent(string questKey, int textIndex)
    {
        gameObject.SetActive(true);
        if (botChatTextsCache.TryGetValue(questKey, out BotChatText[] botChatTexts))
        {
            BotChatText = botChatTexts;
            isInitialBotChat = false;
            StartDisplayTextOverTime(BotChatText[textIndex].text);
        }
    }

    // ========================================Implement IQuest2Listener
    public void OnQuest2Started()
    {
        HandleQuestEvent("BotQuest2", 0);
    }

    public void OnQuest2Completed()
    {
        HandleQuestEvent("BotQuest2", 1);
    }

    public void OnQuest2ProgressUpdated(int a) { }

    // ========================================Implement IQuest1Listener
    public void OnQuest1Started()
    {
        HandleQuestEvent("BotQuest1", 0);
    }

    public void OnQuest1Completed()
    {
        HandleQuestEvent("BotQuest1", 1);
    }

    // ========================================Implement IQuest3Listener
    public void OnQuest3Started()
    {
        HandleQuestEvent("BotQuest3", 0);
    }

    public void OnQuest3Completed()
    {
        HandleQuestEvent("BotQuest3", 1);
    }

    // ========================================Implement IQuest4Listener
    public void OnQuest4Started()
    {
        HandleQuestEvent("BotQuest4", 0);
    }

    public void OnQuest4Completed()
    {
        HandleQuestEvent("BotQuest4", 1);
    }
}
