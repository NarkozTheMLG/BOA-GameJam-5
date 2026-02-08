using UnityEngine;

public class TestDialogue : MonoBehaviour
{
    public Dialogue myConversation;

    private void Start()
    {

        StartTalking();

    }
    public void StartTalking()
    {
        DialogueManager.Instance.StartDialogue(myConversation);
    }
}