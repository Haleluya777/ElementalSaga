using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : PoolAble, IRoomInitable, IInteractable
{
    [SerializeField] private List<RoomInfo> rooms = new List<RoomInfo>();
    [SerializeField] private RoomInfo thisRoom;
    [SerializeField] private int total;

    private void Awake()
    {
        WeightInit();

        //디버깅용
        RandomRoom();
    }

    private void WeightInit()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            total += rooms[i].weight;
        }
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

    public void RandomRoom()
    {
        thisRoom = SetRoom();
    }

    private RoomInfo SetRoom()
    {
        int weight = 0;
        int num = 0;

        num = Random.Range(0, total) + 1;
        for (int i = 0; i < rooms.Count; i++)
        {
            weight += rooms[i].weight;
            if (num <= weight)
            {
                RoomInfo temp = new RoomInfo(rooms[i]);
                return temp;
            }
        }
        return null;
    }
}
