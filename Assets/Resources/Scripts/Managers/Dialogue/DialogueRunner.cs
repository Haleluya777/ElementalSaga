using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KoreanTyper;
using System.Linq;
using TMPro;
using System.Text;

public class DialogueRunner : MonoBehaviour, IDataInitializable
{
    [Header("DialogueParser")]
    [SerializeField] private DialogueParser parser;

    [Header("DialogueFile")]
    [SerializeField] public TextAsset DialogueFile;

    [SerializeField] private TextMeshProUGUI DialogueText;

    public Dictionary<string, TextMeshProUGUI> DialogueTextDic = new Dictionary<string, TextMeshProUGUI>();

    private WaitForSeconds duration = new WaitForSeconds(.05f);

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
        //Debug.Log(currentLineNum);
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

            case "I":
                string[] charNames = line.Detail.Replace(" ", "").Split('|');
                foreach (var name in charNames)
                {
                    Debug.Log(name);
                }
                GameManager.instance.eventManager.InitDialogueCharacter(charNames);
                currentLineNum++;
                ProccessNextLine();
                return;

            default:
                Debug.LogWarning("알 수 없는 명령어.");
                currentLineNum++;
                ProccessNextLine();
                return;
        }
        currentLineNum++;
    }

    private void RunningDialogue(DialogueParser.ParsedLine line)
    {
        if (line.Detail.Contains("\\n")) line.Detail = line.Detail.Replace("\\n", "\n");
        //Debug.Log(line.Detail);
        DialogueText = DialogueTextDic[line.Actor];
        StartCoroutine(TypingTxt(line.Detail));
    }

    private IEnumerator TypingTxt(string args)
    {
        //isWaiting = true;

        for (int i = 0; i < args.GetTypingLength() + 1; i++)
        {
            DialogueText.text = args.Typing(i);
            yield return duration;
        }

        yield return null;
        //isWaiting = false;
        //currentLineNum++;

        // if (currentState == RunnerState.Auto || currentState == RunnerState.Skip)
        // {
        //     yield return currentWaitDialogueAutoProccess;
        //     ProccessNextLine();
        // }
    }
}