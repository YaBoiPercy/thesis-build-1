using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_dialogueTrigger : MonoBehaviour
{
    public scr_dialogueScript dialogue;

    public void TriggerDialogue()
    {
        FindObjectOfType<scr_dialogueManager>().StartDialogue(dialogue);
    }

    public void ExitDialogue()
    {
        FindObjectOfType<scr_dialogueManager>().EndDialogue();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerDialogue();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ExitDialogue();
        }
    }
}
