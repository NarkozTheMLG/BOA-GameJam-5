
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;  
    public Sprite portrait;     
    [TextArea(3, 10)]
    public string text;        
}

[System.Serializable]
public class Dialogue
{
    public DialogueLine[] lines;
}