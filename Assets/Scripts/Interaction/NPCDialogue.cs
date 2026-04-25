using UnityEngine;
using TMPro;
using System.Collections;

[System.Serializable]
public class DialogueLine
{
    public string speaker;
    
    [TextArea(2,5)]
    public string text;
}

public class NPCDialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text speakerText;
    public TMP_Text dialogueText;

    public DialogueLine[] lines;

    public float typingSpeed = 0.03f;

    private int currentIndex = 0;
    private bool playerInRange = false;
    private bool isTyping = false;
    private Coroutine typingRoutine;

void Start()
{
    if (dialoguePanel != null)
        dialoguePanel.SetActive(false);
}

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping)
                CompleteLine();
            else
                NextLine();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            playerInRange = true;
            currentIndex = 0;
            dialoguePanel.SetActive(true);
            ShowLine();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            EndDialogue();
        }
    }

    void ShowLine()
    {
        speakerText.text = lines[currentIndex].speaker;

        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        typingRoutine = StartCoroutine(TypeText(lines[currentIndex].text));
    }

    IEnumerator TypeText(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void CompleteLine()
    {
        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        dialogueText.text = lines[currentIndex].text;
        isTyping = false;
    }

    void NextLine()
    {
        currentIndex++;

        if (currentIndex < lines.Length)
            ShowLine();
        else
            EndDialogue();
    }

void EndDialogue()
{
    if (typingRoutine != null)
        StopCoroutine(typingRoutine);

    if (dialoguePanel != null)
        dialoguePanel.SetActive(false);

    if (dialogueText != null)
        dialogueText.text = "";

    if (speakerText != null)
        speakerText.text = "";

    playerInRange = false;
    isTyping = false;
}
}