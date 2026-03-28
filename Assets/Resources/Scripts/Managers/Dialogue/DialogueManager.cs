using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour, IDataInitializeable
{
    public DialogueRunner dialogueRunner;
    public DialogueUIManager dialogueUIManager;
    public DialogueFuncManager dialogueFuncManager;
    public GameObject dialogueCanvas;
    public void DataInitialize()
    {

    }
}
