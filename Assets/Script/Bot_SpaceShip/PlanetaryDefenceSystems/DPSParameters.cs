
using UnityEngine;

[CreateAssetMenu(fileName = "DPSParameters", menuName = "Scriptable Objects/DPSParameters")]
public class DPSParameters : ScriptableObject
{
    [Header("AOT")]
    public float cooldownAOT; // Thời gian nạp đạn
    public float damageAOT; // Sát thương gây ra
    public float timeActiveAOT; // Thời gian tồn tại

    [Header("AOM")]
    public float cooldownAOM; // Thời gian nạp đạn
    public float damageAOM; // Sát thương gây ra
    public float timeActiveAOM; // Thời gian tồn tại

    [Header("AOC")]
    public float cooldownAOC; // Thời gian nạp đạn
    public float damageAOC; // Sát thương gây ra
    public float timeActiveAOC; // Thời gian tồn tại
}
