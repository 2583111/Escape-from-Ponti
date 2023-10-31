using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivate : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;

    public void DialogueActivater()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
