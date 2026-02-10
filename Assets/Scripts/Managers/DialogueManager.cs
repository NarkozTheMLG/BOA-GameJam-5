using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    private static HashSet<string> finishedDialoguesInSession = new HashSet<string>();

    [Header("IMPORTANT: Unique ID")]
    public string uniqueID;

    [Header("Prefab Settings")]
    public GameObject dialogueBoxPrefab;

    [Header("End of Dialogue Settings")]
    public GameObject objectToDestroy1;
    public GameObject objectToDestroy2;
    public string itemRewardName;

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
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        sentences = new Queue<DialogueLine>();
    }

    void Start()
    {
        if (!string.IsNullOrEmpty(uniqueID) && finishedDialoguesInSession.Contains(uniqueID))
        {
            if (objectToDestroy1 != null) Destroy(objectToDestroy1);
            if (objectToDestroy2 != null) Destroy(objectToDestroy2);

            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (!string.IsNullOrEmpty(uniqueID) && finishedDialoguesInSession.Contains(uniqueID)) return;

        if (currentDialogueBox != null) Destroy(currentDialogueBox);
        currentDialogueBox = Instantiate(dialogueBoxPrefab);
        isDialogueActive = true;

        Transform canvas = currentDialogueBox.transform.Find("Canvas");
        if (canvas != null)
        {
            portraitImage = canvas.Find("Portrait").GetComponent<Image>();
            nameText = canvas.Find("Name").GetComponent<TextMeshProUGUI>();
            dialogueText = canvas.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        }

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
            if (dialogueText != null) dialogueText.text = currentFullSentence;
            isTyping = false;
            return;
        }

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = sentences.Dequeue();

        if (nameText != null) nameText.text = currentLine.speakerName;

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
        if (dialogueText != null) dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            if (dialogueText != null) dialogueText.text += letter;
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

        if (!string.IsNullOrEmpty(itemRewardName))
        {
            InventoryManager.Instance.AddItem(itemRewardName, 1);
        }

        if (objectToDestroy1 != null) Destroy(objectToDestroy1);
        if (objectToDestroy2 != null) Destroy(objectToDestroy2);

        if (!string.IsNullOrEmpty(uniqueID))
        {
            finishedDialoguesInSession.Add(uniqueID);
        }

        Destroy(gameObject);
    }
}