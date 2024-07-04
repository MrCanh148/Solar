using UnityEngine;

[CreateAssetMenu(fileName = "BotChatText", menuName = "Scriptable Objects/BoxChatText")]
public class BotChatText : ScriptableObject
{
    [Multiline]
    public string text;
}
