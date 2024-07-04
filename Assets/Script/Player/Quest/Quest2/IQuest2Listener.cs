public interface IQuest2Listener
{
    void OnQuest2Started();
    void OnQuest2Completed();
    void OnQuest2ProgressUpdated(int percentage);
}
