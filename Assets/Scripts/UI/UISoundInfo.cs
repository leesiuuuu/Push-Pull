using UnityEngine;

[CreateAssetMenu(fileName = "UISoundInfo", menuName = "ScriptableObjects/UISoundInfo", order = 1)]
public class UISoundInfo : ScriptableObject
{
    public AudioClip ClickSound;
    public AudioClip HoverSound;
}