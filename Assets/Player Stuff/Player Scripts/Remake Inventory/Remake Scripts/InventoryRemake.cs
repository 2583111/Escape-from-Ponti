using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryRemake : MonoBehaviour
{
    private const int SLOTS = 12;
    private List<IInventoryItem> mItems = new List<IInventoryItem>();

    public event EventHandler<InventoryEventArgs> ItemAdded;

    public void AddItem(IInventoryItem item)
    {
        if (mItems.Count < SLOTS)
        {
            // Find the player GameObject by tag.
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                Collider playerCollider = player.GetComponent<Collider>();

                if (playerCollider != null && playerCollider.enabled)
                {
                    // Assuming that the item's GameObject has a Collider component.
                    Collider itemCollider = (item as MonoBehaviour).GetComponent<Collider>();

                    if (itemCollider != null && itemCollider.enabled)
                    {
                        playerCollider.enabled = false;
                        mItems.Add(item);
                        item.OnPickUp();

                        ItemAdded?.Invoke(this, new InventoryEventArgs(item));
                    }
                }
            }
        }
    }

}
