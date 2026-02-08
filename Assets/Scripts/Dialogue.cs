
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;  // e.g. "Hero"
    public Sprite portrait;     // Drag the face image here!
    [TextArea(3, 10)]
    public string text;         // e.g. "I will never sleep!"
}

[System.Serializable]
public class Dialogue
{
    public DialogueLine[] lines;
}