using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour, IInventoryItem
{
    public string Name => "Battery";
    public Sprite _Image;

    public Sprite Image => _Image;

    public void OnPickUp()
    {
        gameObject.SetActive(false);
    }

}
