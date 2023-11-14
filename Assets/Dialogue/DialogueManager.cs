using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Displays")]
    [SerializeField] GameObject interactionPopUp; //Talk-E image
    [SerializeField] GameObject DialogueCanvas; //Dialogue Canvas

    [Header("Code Fields")]
    [SerializeField] TextMeshProUGUI nameText; //Name
    [SerializeField] TextMeshProUGUI dialogueText; //Text in dialogue box
    Queue<string> sentences; //Sentences
    PlayerCamera playerCamera;

    [Header("NPC Dialogue")]
    public string NPCname; //Name
    [TextArea(4, 10)] public string[] NPCsentences; //Sentences
    

    void Start()
    {
        sentences = new Queue<string>();
        playerCamera = FindObjectOfType<PlayerCamera>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            interactionPopUp.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            interactionPopUp.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactionPopUp.activeSelf)
        {
            DialogueCanvas.SetActive(true);
            CursorDisplay();
            StartDialogue();
            playerCamera.sensitivityX = 0;
            playerCamera.sensitivityY = 0;
        }
    }

    public void StartDialogue()
    {
        Debug.Log("Convo with " + NPCname);

        nameText.text = NPCname;

        sentences.Clear();

        foreach (string sentence in NPCsentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }
    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void EndDialogue()
    {
        Debug.Log("End");
        CursorHide();
        DialogueCanvas.SetActive(false);
        playerCamera.sensitivityX = 15;
        playerCamera.sensitivityY = 15;
    }

    public void CursorDisplay()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CursorHide()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
