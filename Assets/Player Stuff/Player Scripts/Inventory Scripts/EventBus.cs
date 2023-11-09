using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static EventBus Instance { get; private set; }

    public event Action<ItemData> onPickUpItem;

    public void PickUpItem(ItemData itemData)
    {
        onPickUpItem?.Invoke(itemData);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // This ensures the object persists across scenes.
        }
        else
        {
            Destroy(gameObject);
        }


    }


}
