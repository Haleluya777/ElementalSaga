using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : PoolAble, IInteractable
{
    public RoomInfo thisRoom;

    public void Init()
    {
        this.GetComponent<SpriteRenderer>().sprite = thisRoom.sprite;
    }

    public void Interaction()
    {
        switch (thisRoom.type)
        {
            case RoomType.Battle:
                Debug.Log("전투 방");
                break;

            case RoomType.EliteBattle:
                Debug.Log("엘리트 전투 방");
                break;

            case RoomType.Boss:
                Debug.Log("보스 방");
                break;

            case RoomType.Heal:
                Debug.Log("회복 방");
                break;

            case RoomType.Shop:
                Debug.Log("상점 방");
                break;

            case RoomType.Treasure:
                Debug.Log("보상 방");
                break;

            case RoomType.Mimic:
                Debug.Log("미믹 방");
                break;

            case RoomType.Puzzle:
                Debug.Log("퍼즐 방");
                break;
        }
    }
}
