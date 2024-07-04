using System;

[System.Serializable]
public class SaveState
{
    [NonSerialized] private const int ACHIEVEMENT_COUNT = 30;

    // For achievement
    public bool DoneAllAchievement { set; get; }
    public byte[] DoneAchievementFlag { set; get; }
    public DateTime lastSaveDate { set; get; }


    public SaveState()
    {
        lastSaveDate = DateTime.Now;
        DoneAllAchievement = false;
        DoneAchievementFlag = new byte[ACHIEVEMENT_COUNT];
    }

}