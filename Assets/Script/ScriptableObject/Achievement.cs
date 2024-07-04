using UnityEngine;

[CreateAssetMenu(fileName = "Achievement", menuName = "Scriptable Objects/Achievement")]
public class Achievemnet : ScriptableObject
{
    public Sprite Icon;
    public string Name;
    public string Info;
}