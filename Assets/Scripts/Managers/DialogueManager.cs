using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("Prefab Settings")]
    public GameObject dialogueBoxPrefab; // Drag your PREFAB here (from the Project folder)

    // Private variables to hold the created object and its parts
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
        // Only listen for clicks if dialogue is active AND the box exists
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        // 1. Create the Box from the Prefab
        if (currentDialogueBox != null) Destroy(currentDialogueBox); // Safety cleanup
        
        currentDialogueBox = Instantiate(dialogueBoxPrefab);
        isDialogueActive = true;

        // 2. Find the UI parts inside the new box automatically
        // We use the exact names from your screenshot: "Canvas/Portrait", etc.
        Transform canvas = currentDialogueBox.transform.Find("Canvas");

        if (canvas != null)
        {
            portraitImage = canvas.Find("Portrait").GetComponent<Image>();
            nameText = canvas.Find("Name").GetComponent<TextMeshProUGUI>();
            dialogueText = canvas.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("Could not find 'Canvas' inside the DialogueBox prefab!");
        }

        // 3. Start the Logic
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
        
        if (currentDialogueBox != null)
        {
            Destroy(currentDialogueBox);
        }
        
        Debug.Log("End of conversation.");
    }
}