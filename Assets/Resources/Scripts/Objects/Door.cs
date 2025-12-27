using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : PoolAble, IInteractable
{
    [SerializeField] private List<int> enemyList = new List<int>();
    public DoorInfo thisRoom;

    public void Init()
    {
        this.GetComponent<SpriteRenderer>().sprite = thisRoom.sprite;
    }

    public void Interaction()
    {
        switch (thisRoom.type)
        {
            case DoorType.Battle:
                Debug.Log("전투 방");
                GameManager.instance.stageManager.currentRoomId = 0;
                GameManager.instance.stageManager.StageStart();
                break;

            case DoorType.EliteBattle:
                Debug.Log("엘리트 전투 방");
                GameManager.instance.stageManager.currentRoomId = 0;
                GameManager.instance.stageManager.StageStart();
                break;

            case DoorType.Boss:
                Debug.Log("보스 방");
                GameManager.instance.stageManager.currentRoomId = 0;
                GameManager.instance.stageManager.StageStart();
                break;

            case DoorType.Heal:
                Debug.Log("회복 방");
                GameManager.instance.stageManager.currentRoomId = 0;
                GameManager.instance.stageManager.StageStart();
                break;

            case DoorType.Shop:
                Debug.Log("상점 방");
                GameManager.instance.stageManager.currentRoomId = 0;
                GameManager.instance.stageManager.StageStart();
                break;

            case DoorType.Treasure:
                Debug.Log("보상 방");
                GameManager.instance.stageManager.currentRoomId = 0;
                GameManager.instance.stageManager.StageStart();
                break;

            case DoorType.Mimic:
                Debug.Log("미믹 방");
                GameManager.instance.stageManager.currentRoomId = 0;
                GameManager.instance.stageManager.StageStart();
                break;

            case DoorType.Puzzle:
                Debug.Log("퍼즐 방");
                GameManager.instance.stageManager.currentRoomId = 0;
                GameManager.instance.stageManager.StageStart();
                break;
        }
    }
}
