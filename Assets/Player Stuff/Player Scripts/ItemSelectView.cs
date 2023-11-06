using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSelectView : MonoBehaviour
{
    private RectTransform outlines;
    public float speed = 25f;

    private void Awake()
    {
        outlines = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var Selected = EventSystem.current.currentSelectedGameObject;

        if (Selected == null) return;


        transform.position = Vector3.Lerp(transform.position, Selected.transform.position, speed * Time.deltaTime);
        
        var otherRect = Selected.GetComponent<RectTransform>();

        outlines.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, otherRect.rect.size.x + 5);
        outlines.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, otherRect.rect.size.y + 5);
    }
}
