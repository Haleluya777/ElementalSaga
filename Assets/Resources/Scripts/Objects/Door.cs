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

    void OnEnable()
    {
        if (GameManager.instance is not null)
        {
            GameManager.instance.eventManager.SelectDoor += ReleaseObject;
        }
    }

    void OnDisable()
    {
        if (GameManager.instance is not null)
        {
            GameManager.instance.eventManager.SelectDoor -= ReleaseObject;
        }
    }

    public void Interaction()
    {
        //Debug.Log("할렐루야");
        switch (thisRoom.type)
        {
            case RoomType.Battle:
                Debug.Log("전투 방");
                GameManager.instance.stageManager.currentRoomId = 0;
                break;

            case RoomType.EliteBattle:
                Debug.Log("엘리트 전투 방");
                GameManager.instance.stageManager.currentRoomId = 0;
                break;

            case RoomType.Boss:
                Debug.Log("보스 방");
                GameManager.instance.stageManager.currentRoomId = 4;
                break;

            case RoomType.Heal:
                Debug.Log("회복 방");
                GameManager.instance.stageManager.currentRoomId = 0;
                break;

            case RoomType.Shop:
                Debug.Log("상점 방");
                GameManager.instance.stageManager.currentRoomId = 1;
                break;

            case RoomType.Treasure:
                Debug.Log("보상 방");
                GameManager.instance.stageManager.currentRoomId = 0;
                break;

            case RoomType.Mimic:
                Debug.Log("미믹 방");
                GameManager.instance.stageManager.currentRoomId = 0;
                break;

            case RoomType.Puzzle:
                Debug.Log("퍼즐 방");
                GameManager.instance.stageManager.currentRoomId = 0;
                break;
        }
        GameManager.instance.stageManager.StageStart(thisRoom.type);
        GameManager.instance.stageManager.stage++;
        GameManager.instance.eventManager.ReleaseDoor();
    }
}
