using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static EventBus Instance { get; private set; }

    public event Action<ItemData> onPickUpItem;
    public event Action<ItemData> onItemUsed;

    public void PickUpItem(ItemData itemData)
    {
        onPickUpItem?.Invoke(itemData);
    }
    internal void UseItem(ItemData item)
    {
        onItemUsed?.Invoke(item);
    }
   
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }


}
