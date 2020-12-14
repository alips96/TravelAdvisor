using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Country", menuName = "Assets/ScriptibleObjects/New Country")]
public class Country : ScriptableObject
{
    public List<string> AllRegions;
}