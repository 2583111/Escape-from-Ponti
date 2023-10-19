using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject / Item")]
public class Items : ScriptableObject
{
    public bool stackable = true;
    public Sprite itemSprite;
    public int cost;




}
