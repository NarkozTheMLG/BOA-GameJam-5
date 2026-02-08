using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI Components")]
    public GameObject dialogueBox;      // The whole background panel
    public Image portraitImage;         // The Image on the left
    public TextMeshProUGUI nameText;    // Name text above message
    public TextMeshProUGUI dialogueText;// The actual message

    private Queue<DialogueLine> sentences;
    private bool isTyping = false;
    private string currentFullSentence = "";

    void Awake()
    {
        if (Instance == null) Instance = this;
        sentences = new Queue<DialogueLine>();
    }

    void Start()
    {
        dialogueBox.SetActive(false); 
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Iclickled!!!");
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueBox.SetActive(true);
        sentences.Clear();

        foreach (DialogueLine line in dialogue.lines)
        {
            sentences.Enqueue(line);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        Debug.Log("cispdeopkfpwoekfweopfk");
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentFullSentence;
            isTyping = false;
            return;
        }

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = sentences.Dequeue();

        nameText.text = currentLine.speakerName;

        if (currentLine.portrait != null)
        {
            portraitImage.sprite = currentLine.portrait;
            portraitImage.enabled = true;
        }
        else
        {
            portraitImage.enabled = false;
        }

        currentFullSentence = currentLine.text;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentFullSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        dialogueBox.SetActive(false);
        Debug.Log("End of conversation.");
        Destroy(dialogueBox);
    }
}