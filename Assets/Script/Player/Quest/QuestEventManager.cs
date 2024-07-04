using System.Collections.Generic;
using UnityEngine;

public class QuestEventManager : MonoBehaviour
{
    private static QuestEventManager _instance;
    public static QuestEventManager Instance => _instance;

    private List<IQuestEvent> questEventListeners = new List<IQuestEvent>();
    private List<IQuest2Listener> quest2Listeners = new List<IQuest2Listener>();
    private List<IQuest1Listenner> quest1Listeners = new List<IQuest1Listenner>();
    private List<IQuest3Listenner> quest3Listeners = new List<IQuest3Listenner>();
    private List<IQuest4Listenner> quest4Listeners = new List<IQuest4Listenner>();

    private void Awake()
    {
        _instance = this;
    }

    // ========================================= For IQuestEvent ==================================================
    public void RegisterQuestEvent(IQuestEvent questEvent)
    {
        if (!questEventListeners.Contains(questEvent))
        {
            questEventListeners.Add(questEvent);
        }
    }

    public void UnregisterQuestEvent(IQuestEvent questEvent)
    {
        if (questEventListeners.Contains(questEvent))
        {
            questEventListeners.Remove(questEvent);
        }
    }

    public void NotifyQuestEnter(Quest quest)
    {
        foreach (var listener in questEventListeners)
        {
            listener.OnQuestEnter(quest);
        }
    }

    // ============================================== For IQuest2Listener ====================================================
    public void RegisterListener(IQuest2Listener listener)
    {
        if (!quest2Listeners.Contains(listener))
        {
            quest2Listeners.Add(listener);
        }
    }

    public void UnregisterListener(IQuest2Listener listener)
    {
        if (quest2Listeners.Contains(listener))
        {
            quest2Listeners.Remove(listener);
        }
    }

    public void NotifyQuestStarted()
    {
        foreach (var listener in quest2Listeners)
        {
            listener.OnQuest2Started();
        }
    }

    public void NotifyQuestCompleted()
    {
        foreach (var listener in quest2Listeners)
        {
            listener.OnQuest2Completed();
        }
    }

    public void NotifyQuestProgressUpdated(int percentage)
    {
        foreach (var listener in quest2Listeners)
        {
            listener.OnQuest2ProgressUpdated(percentage);
        }
    }

    // ==================================== For IQuest1Listenner =====================================
    public void Register1Listener(IQuest1Listenner listener)
    {
        if (!quest1Listeners.Contains(listener))
        {
            quest1Listeners.Add(listener);
        }
    }

    public void Unregister1Listener(IQuest1Listenner listener)
    {
        if (quest1Listeners.Contains(listener))
        {
            quest1Listeners.Remove(listener);
        }
    }

    public void NotifyQuest1Started()
    {
        foreach (var listener in quest1Listeners)
        {
            listener.OnQuest1Started();
        }
    }

    public void NotifyQuest1Completed()
    {
        foreach (var listener in quest1Listeners)
        {
            listener.OnQuest1Completed();
        }
    }

    // ==================================== For IQuest3Litener ======================================
    public void Register3Listener(IQuest3Listenner listener)
    {
        if (!quest3Listeners.Contains(listener))
        {
            quest3Listeners.Add(listener);
        }
    }

    public void Unregister3Listener(IQuest3Listenner listener)
    {
        if (quest3Listeners.Contains(listener))
        {
            quest3Listeners.Remove(listener);
        }
    }

    public void NotifyQuest3Started()
    {
        foreach (var listener in quest3Listeners)
        {
            listener.OnQuest3Started();
        }
    }

    public void NotifyQuest3Completed()
    {
        foreach (var listener in quest3Listeners)
        {
            listener.OnQuest3Completed();
        }
    }

    // ========================================= For IQuest4Litener ======================================
    public void Register4Listener(IQuest4Listenner listener)
    {
        if (!quest4Listeners.Contains(listener))
        {
            quest4Listeners.Add(listener);
        }
    }

    public void Unregister4Listener(IQuest4Listenner listener)
    {
        if (quest4Listeners.Contains(listener))
        {
            quest4Listeners.Remove(listener);
        }
    }

    public void NotifyQuest4Started()
    {
        foreach (var listener in quest4Listeners)
        {
            listener.OnQuest4Started();
        }
    }

    public void NotifyQuest4Completed()
    {
        foreach (var listener in quest4Listeners)
        {
            listener.OnQuest4Completed();
        }
    }
}
