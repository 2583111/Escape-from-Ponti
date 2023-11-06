using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InventoryRemake inventory;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Player"))
        {
            IInventoryItem item = hit.collider.GetComponent<IInventoryItem>();
            if (item != null)
            {
                inventory.AddItem(item);
            }
        }
    }
}
