using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KoreanTyper;
using System.Linq;
using TMPro;
using DG.Tweening;
using System.Text.RegularExpressions;

public class DialogueRunner : MonoBehaviour, IDataInitializeable
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI SpeakerName;
    [SerializeField] private TextMeshProUGUI DialogueText;
    [SerializeField] private Image NextImg;
    //[SerializeField] private GameObject ChoiceOptionPanel;

    [Header("DialogueBox 관련 요소들")]
    [SerializeField] private GameObject DialoguePanel; //대화창 전체 부모 오브젝트.
    [SerializeField] private GameObject ChoiceButtonPrefab; //선택지 버튼 프리팹
    [SerializeField] private Transform OptionContainer; // ChoiceButton들의 부모 오브젝트

    //DialogueLog 관련 요소들.
    [SerializeField] private GameObject DialogueLogPanel; //대화 로그 창 전체 부모 오브젝트.
    [SerializeField] private GameObject DialogueLogObj; //대화 로그 창 오브젝트.
    [SerializeField] private Transform DialogueLogContainer; //대화 로그 창의 부모 오브젝트.

    [Header("DialogueFile")]
    [SerializeField] public TextAsset DialogueFile;

    [Header("DialogueParse")]
    [SerializeField] private DialogueParser parser;

    [Header("DialogueMenu")]
    [SerializeField] private bool autoTrigger; //대화가 자동으로 진행될지 체크하는 트리거
    [SerializeField] private bool skipTrigger; //대화가 스킵 모드로 진행될지 체크하는 트리거

    [SerializeField] private float settedDialogueTextSpeed; //설정에서 변경된 Dialogue텍스트 진행 속도.
    [SerializeField] private float settedAutoProccessTime; //설정에서 변경된 자동 대화 진행 속도.

    [Header("DialogueCharacters")] //대화에 등장하는 캐릭터 관련 요소들.
    [SerializeField] private GameObject CharacterPrefab; //캐릭터 베이스 프리팹
    [SerializeField] private Transform characterParent; //캐릭터 베이스 프리팹의 부모 오브젝트. Instantiate용.
    [SerializeField] private CharacterMap characterMap;
    [SerializeField] public Dictionary<int, GameObject> characters = new Dictionary<int, GameObject>(); //대화에 등장하는 캐릭터 오브젝트들.
    [SerializeField] private List<GameObject> leftChars = new List<GameObject>();
    [SerializeField] private List<GameObject> rightChars = new List<GameObject>();

    private const float DIALOGUE_TEXT_SPEED_SKIP = .01f; //텍스트 진행 속도 (스킵 모드)
    private const float DIALOGUE_TEXT_AUTOPROCCESS_SKIP = .01f; //자동 텍스트 넘김 지연 시간. (빠른 모드)

    private WaitForSeconds currentWaitDialogueProccessSpeed; //현재 Dialogue 진행 속도에 쓰는 WaitForSeconds
    private WaitForSeconds currentWaitDialogueAutoProccess; //현재 Dialogue 자동 진행에 쓰는 WaitForSeconds

    private WaitForSeconds skipedWaitDialogueProccessSpeed; //스킵 모드일 때, Dialogue 텍스트 진행 속도에 쓰는 WaitForSeconds
    private WaitForSeconds skipedWaitDialogueAutoProccess; //스킵 모드일 떄, Dialogue 자동 진행에 쓰는 WaitForSeconds

    private WaitForSeconds settedWaitDialogueAutoProccess; //스킵 모드가 아닐 때, Dialogue 자동 진행에 쓰는 WaitForSeconds (설정된 텍스트 속도로 지정함.)
    private WaitForSeconds settedWaitDialogueProccessSpeed; //스킵 모드가 아닐 때, Dialogue 텍스트 진행에 쓰는 WaitForSeconds (설정된 텍스트 속도로 지정.)

    private List<DialogueParser.ParsedLine> scriptLine;
    private int currentLineNum = 0;
    int currentCharId; //현재 말하고 있는 캐릭터의 id값. 0은 나레이션.
    [SerializeField] private bool isWaiting; //대화 일시 정지
    public bool isRunning; //대화가 진행중인지 체크하는 변수.
    public bool isPause;

    private void Start()
    {
        //parser.Parse(DialogueFile.text);
        //임시
        //CharacterInit(3);
        //RunDialogue();
    }

    public void DataInitialize()
    {
        isWaiting = false;
        isRunning = false;

        settedDialogueTextSpeed = .065f;
        settedAutoProccessTime = .6f;
        currentLineNum = 0;
        // currentWaitDialogueProccessSpeed = new WaitForSeconds(settedDialogueTextSpeed);
        // currentWaitDialogueAutoProccess = new WaitForSeconds(settedAutoProccessTime);

        // skipedWaitDialogueAutoProccess = new WaitForSeconds(DIALOGUE_TEXT_AUTOPROCCESS_SKIP);
        // skipedWaitDialogueProccessSpeed = new WaitForSeconds(DIALOGUE_TEXT_SPEED_SKIP);

        // settedWaitDialogueAutoProccess = new WaitForSeconds(settedAutoProccessTime);
        // settedWaitDialogueProccessSpeed = new WaitForSeconds(settedDialogueTextSpeed);
    }

    void Update()
    {
        //Debug.Log(currentLineNum);
        //DialogueStateAction();

        if (isRunning)
        {
            // if (Input.GetKeyDown(KeyCode.Z))
            // {
            //     //Debug.Log(GameManager.instance.dataManager.runningCharacters[1]);
            // }

            // if (Input.GetKey(KeyCode.LeftControl))
            // {
            //     currentState = RunnerState.Skip;
            //     //if (!isWaiting) ProccessNextLine();
            // }
            // else if (Input.GetKeyUp(KeyCode.LeftControl))
            // {
            //     currentState = RunnerState.Normal;
            // }

            if (isWaiting) return;

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (currentLineNum != 0)
                {
                    ProccessNextLine();
                }
            }
        }
    }

    //출력 모드에 따른 텍스트 속도 변경.
    // private void DialogueStateAction()
    // {
    //     switch (currentState)
    //     {
    //         case RunnerState.Normal:
    //             currentWaitDialogueProccessSpeed = settedWaitDialogueProccessSpeed;
    //             break;

    //         case RunnerState.Auto:
    //             currentWaitDialogueProccessSpeed = settedWaitDialogueProccessSpeed;
    //             currentWaitDialogueAutoProccess = settedWaitDialogueAutoProccess;
    //             break;

    //         case RunnerState.Skip:
    //             currentWaitDialogueProccessSpeed = skipedWaitDialogueProccessSpeed;
    //             currentWaitDialogueAutoProccess = skipedWaitDialogueAutoProccess;
    //             break;
    //     }
    // }

    //설정 메뉴에서 변경된 텍스트 속도에 따른 변수 변경.
    public void SettingChangeTextSpeed(float value)
    {
        settedDialogueTextSpeed = value;
        settedWaitDialogueProccessSpeed = new WaitForSeconds(settedDialogueTextSpeed);
    }

    //설정 메뉴에서 변경된 자동 대화 진행 속도에 따른 변수 변경.
    public void SettingAutoNextLineSpeed(float value)
    {
        settedAutoProccessTime = value;
        settedWaitDialogueAutoProccess = new WaitForSeconds(settedAutoProccessTime);
    }

    public void CharacterInit(int index, string[] positions) //대화에 등장하는 모든 캐릭터 오브젝트 초기화.
    {
        int num = 0;
        foreach (Transform character in characterParent)
        {
            character.gameObject.SetActive(false);
        }

        for (int i = 0; i < 32; i++)
        {
            if (((index >> i) & 1) == 1)
            {
                //캐릭터 UI이미지 생성 및, 캐릭터 맵 정보에 따른 오브젝트 데이터 초기화.
                GameObject character = Instantiate(CharacterPrefab, characterParent);
                CharacterData data = characterMap.GetCharacter(i);

                character.GetComponent<Image>().sprite = data.characterSpriteMap.sprites["Default"]; //기본 표정으로 초기화.
                character.name = data.characterName;

                //character.SetActive(false);
                if (num < positions.Length)
                {
                    //위치 초기화.
                    Debug.Log($"할렐루야 위치 : {LocalGameManager.instance.dialogueManager.dialogueFuncManager.CalculatePos(positions[num].Trim())}");
                    RectTransform rectTransform = character.GetComponent<RectTransform>();
                    Debug.Log(positions[num]);
                    rectTransform.anchoredPosition = new Vector2(LocalGameManager.instance.dialogueManager.dialogueFuncManager.CalculatePos(positions[num].Trim()), -300);

                    if (rectTransform.anchoredPosition.x > 0) rightChars.Add(character);
                    else leftChars.Add(character);
                }
                else
                {
                    // 기본 위치
                    character.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -300);
                }
                //비활성화.
                //character.SetActive(false); 
                if (characters.ContainsKey(data.id)) continue; //딕셔너리에 값이 존재하면 패스, 없으면 값 추가.
                characters.Add(data.id, character);

                num++;
            }
        }
    }
    //타임라인 시그널에 쓰일 이벤트--------------------------
    public void DialoguePause()
    {
        Debug.Log("대화 일시정지");
        isPause = true;
    }

    public void DialogueResume()
    {
        Debug.Log("대화 재개");
        isPause = false;
        ProccessNextLine();
        //GameManager.instance.timeLineManager.TimeLinePause();
        //if ((currentState == RunnerState.Auto || currentState == RunnerState.Skip) && !isWaiting)
        //{
        //    ProccessNextLine();
        //}
    }
    //======================================================

    public void RunDialogue()
    {
        //currentLineNum = lineNum;
        isRunning = true;
        parser.Parse(DialogueFile.text);
        currentCharId = -1;
        if (DialogueFile != null)
        {
            scriptLine = parser.Parse(DialogueFile.text);
        }

        DialoguePanel.SetActive(true);
        //임시.
        //CharacterInit(512);
        //======
        ProccessNextLine();
    }

    public void EndDialogue()
    {
        //var dataManager = GameManager.instance.dataManager;
        isRunning = false;
        //dataManager.runningCharacters.Clear();
        //currentLineNum = 0;
        currentCharId = -1;
        DialoguePanel.SetActive(false);
        LocalGameManager.instance.timeLineManager.TimeLinePlay();
        //dataManager.dialogueLog.Clear(); //지나간 대화 로그 삭제

        //GameManager.instance.uiManager.FadeDailogueEnd(GameManager.instance.uiManager.dialogueUIManager.gameObject); //페이드 인아웃 연출 및 다음 고정 회화 확인.
    }
    //------------------------------------------

    public void ProccessNextLine()
    {
        Debug.Log(currentLineNum);
        if (currentLineNum >= scriptLine.Count) //현재 라인 넘버가 실행중인 대화 스크립트의 총 줄 개수보다 많거나 같으면.
        {
            EndDialogue(); //대화 종료.
            return;
        }

        if (isWaiting || isPause) return; //대기중이거나 대화 일시 중지 상태일때도 메서드 종료.
        DialogueParser.ParsedLine line = scriptLine[currentLineNum];
        //Debug.Log(line);

        switch (line.Action)
        {
            case "Char": //명령어가 Char일 경우, 캐릭터 초기화 후 다음 줄 실행.
                {
                    int numbers = int.Parse(line.Detail.condition.Trim());
                    string[] positions = line.Detail.result.Split('|');
                    CharacterInit(numbers, positions);
                    currentLineNum++;
                    RunningOtherNode(line);
                    ProccessNextLine();
                    return;
                }

            case "T": //액션 노드가 T일 경우, 대사를 출력.
                {
                    RunningDialogue(line);
                    break;
                }

            case "S": //액션 노드가 S일 경우, 선택지를 제시한 후, 고른 선택지에 따라 줄 이동.
                {
                    currentCharId = -1;
                    HandleChoices(line.Detail.condition.Split('|'), line.Detail.result.Split('|'), line);
                    return;
                }

            case "If": //액션 노드가 If일 경우, 조건 체크 및 조건에 부합한지 부합하지 않은지 체크한 후, 줄 이동.
                {
                    currentCharId = -1;
                    CheckingCondition(line.Detail.condition.Split('|'), line.Detail.result.Split('|'));
                    return;
                }

            case "": //액션 노드에 아무것도 없는 경우. T행동의 연장선으로 간주, Detail의 Result를 출력한다. T행동의 연장이 아니더라도, 다른 노드를 실행.
                {
                    //if (currentCharId == -1) break;
                    //Debug.Log(currentLineNum);
                    RunningDialogue(line);
                    break;
                }

            case "J": //액션 노드가 J일 경우, 조건을 만족할 시, DialogueAsset을 변경, 조건을 만족하지 않으면 일정 줄 만큼 -이동.
                {
                    currentCharId = -1;
                    JumpDialogue(line);
                    return;
                }

            case "Flag":
                {
                    var dataManager = GameManager.instance.dataManager;
                    currentLineNum++;
                    ProccessNextLine();
                    break;
                }
            case "End":
                {
                    Debug.Log("끄읏");
                    EndDialogue();
                    return;
                }

            case "Random":
                {
                    List<int> numbers = line.Detail.condition.Split('|').Select(int.Parse).ToList();
                    var jumpNum = numbers[UnityEngine.Random.Range(0, numbers.Count)];
                    Debug.Log($"넘어가기전 줄 {currentLineNum}");
                    currentLineNum += jumpNum;
                    Debug.Log($"넘어갈 줄의 수 {jumpNum}, 현재 줄 : {currentLineNum}");
                    ProccessNextLine();
                    return;
                }

            case "CU":
                {
                    int index;
                    int.TryParse(line.Detail.condition, out index);

                    for (int i = 0; i < 32; i++)
                    {
                        if (((index >> i) & 1) == 1)
                        {
                            //Debug.Log($"해당 캐릭터 찾음. id값 : {i}");
                            characters[i].SetActive(false);
                            //Destroy(GameManager.instance.dataManager.runningCharacters[i + 1].obj);
                        }
                    }
                    currentLineNum++;
                    //Debug.Log(currentLineNum);
                    ProccessNextLine();
                    return;
                }

            default:
                {
                    Debug.LogWarning($"알 수 없는 액션: {line.Action} (라인 {currentLineNum})");
                    currentLineNum++;
                    currentCharId = -1;
                    ProccessNextLine();
                    return;
                }
        }

        //Debug.Log($"체킹! Action: {line.Action}, Line:{currentLineNum}");
        RunningOtherNode(line);
        currentLineNum++;
    }

    private void JumpDialogue(DialogueParser.ParsedLine line) //DialogueAsset 변경.
    {
        try
        {
            int jumpLine = int.Parse(line.Detail.result);
            currentLineNum = currentLineNum - jumpLine;
        }
        catch
        {
            Debug.LogWarning($"잘못 된 내용");
        }
        finally
        {
            EndDialogue();
        }
    }

    private void RunningOtherNode(DialogueParser.ParsedLine line)
    {
        //if (line.BGM != "") GameManager.instance.dialogueFunc.ChangeBGM(line.BGM); //브금 변경.

        // if (line.CutScene != "")
        // {
        //     GameManager.instance.uiManager.dialogueUIManager.CutSceneImg.gameObject.SetActive(true);
        //     GameManager.instance.uiManager.dialogueUIManager.CutSceneImg.sprite = GameManager.instance.dataManager.MainCharacterData.characterCutScne.cutSceneMap[int.Parse(line.CutScene)];
        // }
        // else
        // {
        //     GameManager.instance.uiManager.dialogueUIManager.CutSceneImg.gameObject.SetActive(false);
        // }
        //currentLineNum++;
    }

    private void RunningDialogue(DialogueParser.ParsedLine line)
    {
        //Debug.Log(line.Detail.condition);
        var shakeEffect = DialogueText.GetComponent<DialogueTextManager>();
        shakeEffect.shakeRanges.Clear();

        // 딜레이 태그를 제외한 텍스트에서 흔들림 범위를 계산해야 인덱스가 맞음
        string delayPattern = @"\((\d+(\.\d+)?)\)";
        string textWithoutDelays = Regex.Replace(line.Detail.condition, delayPattern, "");

        var matches = Regex.Matches(textWithoutDelays, @"\{(.*?)\}");
        int tagOffset = 0;

        foreach (Match match in matches)
        {
            string innerText = match.Groups[1].Value;

            int startIdx = match.Index - tagOffset;
            int endIdx = startIdx + innerText.Length - 1;

            shakeEffect.shakeRanges.Add((startIdx, endIdx));

            tagOffset += 2;
        }

        //대사에 \n이 포함되어 있을 경우, 줄바꿈 처리.
        if (line.Detail.result.Contains("\\n")) line.Detail.condition = line.Detail.condition.Replace("\\n", "\n");
        //대사에 '{}'가 존재할 경우, 대괄호 안의 문자열의 색을 붉은 색으로 변경.
        line.Detail.condition = Regex.Replace(line.Detail.condition, @"\{(.*?)\}", "<color=#FF0000>$1</color>");

        /*Debug.Log(int.Parse(line.Actor.Split('_')[0]));*/

        if (line.Actor == "") //화자 이름이 없을 경우. 나레이션인 경우.
        {
            if (SpeakerName == null) return;
            SpeakerName.gameObject.transform.parent.gameObject.SetActive(false); //화자 이름이 없을 경우, 화자 이름 UI 비활성화.
            //Debug.Log("나레이션이 말을 한다.");
            currentCharId = 0;
        }

        else //화자 이름이 있을 경우. 캐릭터가 말하는 경우.
        {
            SpeakerName.gameObject.transform.parent.gameObject.SetActive(true); //화자 이름이 있을 경우, 화자 이름 UI 활성화.
            string[] actorDetail = line.Actor.Split('_');
            if (actorDetail.Length <= 2) //'_'기준으로 Actor를 나눴을 때의 배열 길이가 2이하일 때. (캐릭터의 id와 이름만 있을 떄)
            {
                SpeakerName.text = actorDetail[1]; //화자 이름만 출력.
            }
            else if (actorDetail.Length > 2)//Actor칸이 ID_이름_이명 형식일 때.
            {
                SpeakerName.text = actorDetail[1] + " | " + actorDetail[2];
            }
            currentCharId = int.Parse(actorDetail[0]);
            CharacterEmphasis(currentCharId, line.Face); //화자 캐릭터 강조.
        }

        StartCoroutine(TypingTxt(line.Detail.condition)); //대사 출력.
        //GameManager.instance.dataManager.dialogueLog.Add(line); //대화 로그에 저장.
    }

    private void CharacterEmphasis(int id, string emotion) //화자 캐릭터의 강조 및 해당 캐릭터 스프라이트 변경(필요시).
    {
        if (id == -1 || emotion == "") return;
        foreach (Transform character in characterParent)
        {
            character.GetComponent<Image>().color = new Color32(140, 140, 140, 255);
            //Debug.Log($"이동해야할 부분 : {LocalGameManager.instance.dialogueManager.dialogueFuncManager.Relocation(character.GetComponent<RectTransform>().anchoredPosition.x)},캐릭터 이름 : {character.gameObject.name}");
            //character.GetComponent<RectTransform>().DOAnchorPosX(LocalGameManager.instance.dialogueManager.dialogueFuncManager.Relocation(character.GetComponent<RectTransform>().anchoredPosition.x), .1f);
        }

        var emphasisChar = characters[id];
        emphasisChar.SetActive(true);

        Image emphasisCharImg = emphasisChar.GetComponent<Image>();

        emphasisCharImg.color = new Color32(255, 255, 255, 255);
        emphasisCharImg.sprite = characterMap.GetCharacter(id).characterSpriteMap.sprites[emotion];
        //화자를 강조하기 위해 캐릭터 배치도 중 가장 왼/오른쪽으로 이동. 그와 동시에 왼/오른쪽에 위치한 캐릭터들의 위치 재배치.

        RectTransform emphasisCharRectTransform = emphasisChar.GetComponent<RectTransform>();
        var funcManager = LocalGameManager.instance.dialogueManager.dialogueFuncManager;

        if (emphasisCharRectTransform.anchoredPosition.x < 0) //강조할 캐릭터가 왼쪽에 위치할 때.
        {
            leftChars.Remove(emphasisChar);
            leftChars.Add(emphasisChar);

            // 리스트 인덱스: 0(바깥) -> 1 -> 2(안쪽/강조캐릭터)
            // Relocation(num)에서 num <= 1 일 때: -baseOffset + (num + 1) * spacing
            // num = -1 이면 -baseOffset (바깥)
            // num = 0 이면 -baseOffset + 100
            // num = 1 이면 -baseOffset + 200 (안쪽)

            for (int i = 0; i < leftChars.Count; i++)
            {
                // i=0 -> num=-1, i=1 -> num=0, i=2 -> num=1
                int relocationNum = i - 1; //-1, 0, 1순서로
                //if (!leftChars[i].activeSelf) continue;
                leftChars[leftChars.Count - i - 1].GetComponent<RectTransform>().DOAnchorPosX(funcManager.Relocation(relocationNum), .5f);
            }
        }
        else if (emphasisCharRectTransform.anchoredPosition.x > 0)//강조할 캐릭터가 오른쪽에 위치할 때.
        {
            rightChars.Remove(emphasisChar);
            rightChars.Add(emphasisChar);

            for (int i = 0; i < rightChars.Count; i++)
            {
                int relocationNum = i + (5 - (rightChars.Count - 1));
                rightChars[i].GetComponent<RectTransform>().DOAnchorPosX(funcManager.Relocation(relocationNum), .5f);
            }
        }

        //강조할 캐릭터를 맨 아래 자식 위치로 이동 시켜 UI상으로 우선순위를 올림.
        emphasisChar.transform.SetAsLastSibling();
    }

    private void CheckingCondition(string[] args, string[] results) //조건 검사 및 참/거짓에 따른 분기 처리.
    {
        for (int i = 0; i < args.Length; i++)
        {
            args[i] = args[i].Trim();
        }
        Debug.Log($"{args[0]}, {args[1]}, {args[2]}");
        if (GameManager.instance.dataManager.operandDic[args[0]] == null) return;
        var leftOperand = GameManager.instance.dataManager.operandDic[args[0]]; //좌측 피 연산자 (데이터 매니저에서 가져옴.)
        var Operator = args[1]; //연산자
        var rightOperand = int.Parse(args[2]); //우측 피 연산자 (int값만 받음.)
        bool condition = false;

        Debug.Log($"{leftOperand} {Operator} {rightOperand}");

        switch (Operator)
        {
            case "<":
                condition = Convert.ToDouble(leftOperand) < Convert.ToDouble(rightOperand);
                break;

            case ">":
                condition = Convert.ToDouble(leftOperand) > Convert.ToDouble(rightOperand);
                break;

            case "==":
                condition = object.Equals(leftOperand, rightOperand);
                break;

            case "<=":
                condition = Convert.ToDouble(leftOperand) <= Convert.ToDouble(rightOperand);
                break;

            case ">=":
                condition = Convert.ToDouble(leftOperand) >= Convert.ToDouble(rightOperand);
                break;

            case "!=":
                condition = !object.Equals(leftOperand, rightOperand);
                break;
        }

        if (condition)
        {
            Debug.Log("조건 충족");
            currentLineNum += int.Parse(results[0].Trim()) + 1; //조건이 참일 경우, result값(점프할 라인 수)만큼 건너뛰고 다음 줄부터 실행.
            ProccessNextLine();
        }

        else
        {
            Debug.Log("조건 불충족");
            currentLineNum += int.Parse(results[1].Trim()) + 1; //조건이 거짓일 경우, result값(점프할 라인 수)만큼 건너뛰고 다음 줄부터 실행. 
            ProccessNextLine();
        }
    }

    private void HandleChoices(string[] selectors, string[] results, DialogueParser.ParsedLine line)
    {
        for (int i = 0; i < selectors.Length; i++)
        {
            selectors[i] = selectors[i].Trim();
            results[i] = results[i].Trim();
        }

        isWaiting = true;
        DialoguePanel.SetActive(false);

        for (int i = 0; i < selectors.Length; i++)
        {
            var buttonObj = Instantiate(ChoiceButtonPrefab, OptionContainer);

            var buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            var button = buttonObj.GetComponent<Button>();
            int index = i;

            buttonText.text = selectors[i];
            button.onClick.AddListener(() => OptionSelected(int.Parse(results[index]), line));
        }
    }

    private void OptionSelected(int lineIndex, DialogueParser.ParsedLine line)
    {
        GameManager.instance.dataManager.dialogueLog.Add(line);
        isWaiting = false;
        //ChoiceOptionPanel.SetActive(false);
        DialoguePanel.SetActive(true);
        //currentLineNum = lineIndex;

        foreach (Transform child in OptionContainer)
        {
            Destroy(child.gameObject);
        }

        Debug.Log($"줄 이동! 이전 줄 : {currentLineNum}, 뛰어넘을 줄{lineIndex + 1}");
        currentLineNum += lineIndex + 1;
        ProccessNextLine();
    }

    //버튼에 할당할 이벤트 집합
    public void GetDialogueLogs() //GameManager에 저장된 이전까지의 대화 로그.
    {
        List<DialogueParser.ParsedLine> logs = GameManager.instance.dataManager.dialogueLog;
        Debug.Log(logs.Count);
        DialogueLogPanel.SetActive(true);

        foreach (Transform child in DialogueLogContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (DialogueParser.ParsedLine log in logs)
        {
            //대화 로그 오브젝트를 생성하는 명령어. 추후 오브젝트 풀링으로 변경 예정.
            var logObj = Instantiate(DialogueLogObj, DialogueLogContainer);
            //var speakerLogText = logObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            var dialogueLogText = logObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            //if (log.Actor == "")
            //{
            //    //speakerLogText.text = "나레이션";
            //}
            //
            //else
            //{
            //    string[] actorDetail = log.Actor.Split('_');
            //    if (actorDetail.Length <= 2) //'_'기준으로 Actor를 나눴을 때의 배열 길이가 2이하일 때. (캐릭터의 id와 이름만 있을 떄)
            //    {
            //        //speakerLogText.text = actorDetail[1]; //화자 이름만 출력.
            //    }
            //    else if (actorDetail.Length > 2)//Actor칸이 ID_이름_이명 형식일 때.
            //    {
            //        //speakerLogText.text = actorDetail[1] + " | " + actorDetail[2];
            //    }
            //}

            dialogueLogText.text = log.Detail.condition;
        }
    }

    //------------
    private IEnumerator TypingTxt(string args)
    {
        isWaiting = true;

        string pattern = @"\((\d+(\.\d+)?)\)";
        MatchCollection matches = Regex.Matches(args, pattern);

        // 태그가 제거된 실제 출력될 문자열
        string fullText = Regex.Replace(args, pattern, "");

        int currentStep = 0;
        int lastProcessedIndex = 0;
        int totalTypingLength = fullText.GetTypingLength();

        foreach (Match match in matches)
        {
            // 태그 직전까지의 텍스트 길이를 통해 타이핑 스텝 계산
            string textBeforeTag = args.Substring(lastProcessedIndex, match.Index - lastProcessedIndex);
            int stepsForThisSegment = textBeforeTag.GetTypingLength();

            for (int i = 0; i < stepsForThisSegment; i++)
            {
                currentStep++;
                DialogueText.text = fullText.Typing(currentStep);
                yield return currentWaitDialogueProccessSpeed;
            }

            // 딜레이 적용
            if (float.TryParse(match.Groups[1].Value, out float delay))
            {
                yield return new WaitForSeconds(delay);
            }

            lastProcessedIndex = match.Index + match.Length;
        }

        // 남은 부분 타이핑
        while (currentStep < totalTypingLength)
        {
            currentStep++;
            DialogueText.text = fullText.Typing(currentStep);
            yield return currentWaitDialogueProccessSpeed;
        }

        DialogueText.text = fullText;
        yield return null;
        isWaiting = false;
        //currentLineNum++;
    }
}