using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    Transform parentAfterDrag;

    public Items item;

    private int count = 1;
    [SerializeField] TextMeshProUGUI countText;
    
    public Transform ParentAfterDrag //The transform of the parent inventory slot allowing the item
                                     //we are dragging to be placed in the same position as the inventory slot it is left on
    {
        get { return parentAfterDrag; }
        set { parentAfterDrag = value; }
    }
    
    public int Count //Getters and setters for the count variable
    {
        get { return count; }
        set { count = value; }
    }

    public void InitialiseItem(Items newItem)
    {
        item = newItem;
        GetComponent<Image>().sprite = newItem.itemSprite;
        RefreshCount();
    }

    public void RefreshCount() //Resets the text holding the quantity of a specific item when it is changed (either through buying or selling more of it)
    {
        countText.text = Count.ToString();
        bool textActive = Count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
       if(eventData.button == PointerEventData.InputButton.Left)
        {
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            GetComponent<Image>().raycastTarget = false;
        }
       
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            transform.position = Input.mousePosition;
        }
            
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            transform.SetParent(parentAfterDrag);
            GetComponent<Image>().raycastTarget = true;
            RefreshCount();
        }
    }

    
}
