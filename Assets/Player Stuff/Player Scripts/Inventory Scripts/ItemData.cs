using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Image itemSprite;
    public string dropItem;
    public ItemCategory itemCategory;

    public enum ItemCategory
    {
        KeyItem,
        Consumable,
        Flashlight,
    }

}
