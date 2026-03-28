using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Hallelujah;
using System;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;

//[ES3Serializable]
public class SystemDataManager : MonoBehaviour, IDataInitializeable
{
    //게임 시스템 데이터를 관리하는 매니저 스크립트.
    //여러 캐릭터들의 호감도, 현재 턴, 플레이어가 행동할 수 있는 ActionPoint등, 게임 내에 존재하는 데이터를 보관 및 조정.

    //캐릭터 및 진행 사항을 저장하는 구조체.
    public struct CharacterDatas
    {
        public GameObject obj;
        //public CharacterData characterData;
    }

    public int maxAP; //[저장 필요]
    //[SerializeField] public CharacterMap characterMap; //캐릭터 데이터 베이스 [저장 필요]
    public Dictionary<int, CharacterDatas> runningCharacters = new Dictionary<int, CharacterDatas>(); //현재 대화에 참여중인 캐릭터들만 [저장 필요]
    public List<DialogueParser.ParsedLine> dialogueLog = new List<DialogueParser.ParsedLine>(); //지나간 대화 로그. [저장 필요]

    public int fixedConversationNum; //몇 번째 고정 대화를 실행할지 저장하는 정보 [저장 필요]

    //대화의 주체가 되는 중심 캐릭터 데이터. 대화 진행 중, 혹은 대화 마지막에 대화 스크립트 변경 시 CharacterMap에서 해당 캐릭터의 변수값을 변경할 접근용으로 사용. (캐릭터가 메인 로비에 있을 경우 값은 0)
    //[저장 필요]
    //public CharacterData MainCharacterData;

    //[ES3NonSerializable]
    //public FixedDialoguesMap fixedDialoguesMap;
    //public SerializedDic_BG backgroundMap; //배경 이미지 데이터 베이스
    public Dictionary<string, object> operandDic = new Dictionary<string, object>(); //조건 체크할 때 쓰는 피연산자.

    //public List<int> characterDialogueNum = new List<int> { 0, 0, 0, 0, 0 }; //캐릭터의 대화 진행 상황. 2진수로 사용할 예정.

    public void DataInitialize()
    {
        fixedConversationNum = 0;
        CheckingFixedDialogue();
    }

    public void CheckingFixedDialogue()
    {
        // int mask = 1 << dailyRoutine.IndexOf(dailyRoutine.Get());
        // int result = fixedConversationList[proccessDatas.Day - 1] & mask;
        // if (result >> dailyRoutine.IndexOf(dailyRoutine.Get()) != 0)
        // {
        //     Debug.Log("고정 대화 있음.");
        //     //대화 UI실행
        //     //GameManager.instance.uiManager.FadeDialogueStart(FxiedDialogueRun);

        // }
        // else
        // {
        //     Debug.Log("고정 대화 없음. 플레이어 행동 가능");
        // }
    }

    private void FxiedDialogueRun()
    {
        //Debug.Log("고정 대화 실행!");
        // if (fixedConversationNum == fixedDialoguesMap.dialogues[proccessDatas.Day - 1].assets.Count) return;
        // LocalGameManager.instance.dialogueRunner.DialogueFile = fixedDialoguesMap.GetDialogues(proccessDatas.Day - 1, fixedConversationNum);
        // MainCharacterData = characterMap.GetCharacter(0);
        // GameManager.instance.dialogueRunner.RunDialogue(0);

        fixedConversationNum++;
    }
}
