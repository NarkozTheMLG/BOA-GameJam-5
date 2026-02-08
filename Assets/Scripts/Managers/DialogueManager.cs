using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("Prefab Settings")]
    public GameObject dialogueBoxPrefab; 

    [Header("End of Dialogue Settings")]
    public GameObject objectToDestroy; // <--- DRAG THE OBJECT HERE IN INSPECTOR
    public string itemRewardName;      // <--- TYPE "Schizophrenia" HERE

    // Private variables for the UI
    private GameObject currentDialogueBox;
    private Image portraitImage;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI dialogueText;

    private Queue<DialogueLine> sentences;
    private bool isTyping = false;
    private string currentFullSentence = "";
    private bool isDialogueActive = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        sentences = new Queue<DialogueLine>();
    }

    void Update()
    {
        // Only listen for clicks if dialogue is active
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            DisplayNextSentence();
        }
    }

    // Call this normally: DialogueManager.Instance.StartDialogue(yourDialogue);
    public void StartDialogue(Dialogue dialogue)
    {
        // 1. Create the Box
        if (currentDialogueBox != null) Destroy(currentDialogueBox);
        currentDialogueBox = Instantiate(dialogueBoxPrefab);
        isDialogueActive = true;

        // 2. Find UI parts inside the prefab
        Transform canvas = currentDialogueBox.transform.Find("Canvas");
        if (canvas != null)
        {
            portraitImage = canvas.Find("Portrait").GetComponent<Image>();
            nameText = canvas.Find("Name").GetComponent<TextMeshProUGUI>();
            dialogueText = canvas.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("Could not find 'Canvas' inside DialogueBox prefab!");
        }

        // 3. Queue Sentences
        sentences.Clear();
        foreach (DialogueLine line in dialogue.lines)
        {
            sentences.Enqueue(line);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            if(dialogueText != null) dialogueText.text = currentFullSentence;
            isTyping = false;
            return;
        }

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = sentences.Dequeue();

        if(nameText != null) nameText.text = currentLine.speakerName;

        if (portraitImage != null)
        {
            if (currentLine.portrait != null)
            {
                portraitImage.sprite = currentLine.portrait;
                portraitImage.enabled = true;
            }
            else
            {
                portraitImage.enabled = false;
            }
        }

        currentFullSentence = currentLine.text;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentFullSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        if(dialogueText != null) dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            if(dialogueText != null) dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(0.02f); 
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        
        // 1. Destroy the UI Box
        if (currentDialogueBox != null)
        {
            Destroy(currentDialogueBox);
        }

        // --- THE LOGIC YOU ASKED FOR ---
        
        // 2. Add Item (if you typed a name in Inspector)
        if (!string.IsNullOrEmpty(itemRewardName))
        {
            InventoryManager.Instance.AddItem(itemRewardName, 1);
            Debug.Log("Reward Added: " + itemRewardName);
        }

        // 3. Destroy the Object (the one you dragged in Inspector)
        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
            Debug.Log("Object Destroyed.");
        }

        Debug.Log("End of conversation.");
    }
}