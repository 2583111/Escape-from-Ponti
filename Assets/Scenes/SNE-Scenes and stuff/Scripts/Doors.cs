using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public Animator door;
    public GameObject openText;

    public bool inReach;

    void Start()
    {
        inReach = false;
        openText.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = true;
            openText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = false;
            openText.SetActive(false);
        }
    }





    void Update()
    {

        if (inReach)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (door.GetBool("Closed"))
                {
                    DoorOpens();
                    openText.SetActive(false);
                }
                else
                {
                    DoorCloses();
                    openText.SetActive(true);
                }
            }
            else
            {
                openText.SetActive(true);
            }
        }
        else
        {
            openText.SetActive(false);
        }
            



        
        
        





    }
    void DoorOpens ()
    {
        Debug.Log("It Opens");
        door.SetBool("Open", true);
        door.SetBool("Closed", false);
       

    }

    void DoorCloses()
    {
        Debug.Log("It Closes");
        door.SetBool("Open", false);
        door.SetBool("Closed", true);
    }


}
