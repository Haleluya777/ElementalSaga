using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using KoreanTyper;
using System.Linq;
using TMPro;
using System.Text;

public class DialogueRunner : MonoBehaviour, IDataInitializable
{
    [Header("DialogueParser")]
    [SerializeField] private DialogueParser parser;

    [Header("DialogueFile")]
    [SerializeField] public TextAsset DialogueFile;

    private List<DialogueParser.ParsedLine> scriptLines;
    private int currentLineNum = 0;

    public void DataInit()
    {

    }

    public void RunDialogue()
    {
        scriptLines = parser.Parse(DialogueFile.text);
        //ProccessNextLine();
    }

    public void EndDialogue()
    {
        Debug.Log("대화 종료.");
    }

    public void ProccessNextLine()
    {
        if (currentLineNum == 0) RunDialogue();
        if (scriptLines.Count <= currentLineNum)
        {
            EndDialogue();
            return;
        }

        DialogueParser.ParsedLine line = scriptLines[currentLineNum];

        switch (line.Action)
        {
            case "T":
                RunningDialogue(line);
                break;

            default:
                currentLineNum++;
                ProccessNextLine();
                break;
        }


        currentLineNum++;
    }

    private void RunningDialogue(DialogueParser.ParsedLine line)
    {
        if (line.Detail.Contains("\\n")) line.Detail = line.Detail.Replace("\\n", "\n");
        Debug.Log(line.Detail);
    }
}