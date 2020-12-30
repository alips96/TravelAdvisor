using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Tip", menuName = "Assets/ScriptibleObjects/New Tip")]
public class Tip : ScriptableObject
{
    public string title;
    public string content;
    public Sprite image;
    public int priority;
}