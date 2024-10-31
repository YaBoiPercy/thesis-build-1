using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class scr_dialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText; //This text type is from Legacy, and seems kind of ugly
    public TextMesh exampleText;
    public Vector3 nameAnchor;
    public Vector3 dialogueAnchor;
    public Vector3 offscreenTextAnchor;

    private Queue<string> sentences;

    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(scr_dialogueScript dialogue)
    {

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences) 
        {
            sentences.Enqueue(sentence);
        }
        nameText.GetComponent<RectTransform>().anchoredPosition = nameAnchor;
        dialogueText.GetComponent<RectTransform>().anchoredPosition = dialogueAnchor;

        DisplayNextSentence();
    }

    public void DisplayNextSentence() 
    {
        if (sentences.Count == 0) 
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    public void EndDialogue()
    {
        Debug.Log("Dialogue ended");
        nameText.GetComponent<RectTransform>().anchoredPosition = offscreenTextAnchor;
        dialogueText.GetComponent<RectTransform>().anchoredPosition = offscreenTextAnchor;
    }
}
