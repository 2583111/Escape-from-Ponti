using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] InventoryManager invenMan;
    [SerializeField] Items itemInfo;
    private Image itemSprite;


    // Start is called before the first frame update
    void Start()
    {
        itemSprite = GetComponentInChildren<Image>();
        itemSprite.sprite = itemInfo.itemSprite;
    }

    public void BuyItem()
    {
        invenMan.AddItem(itemInfo);
    }

}



