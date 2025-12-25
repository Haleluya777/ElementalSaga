using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private List<RoomInfo> rooms = new List<RoomInfo>();
    [SerializeField] private int total;

    void Awake()
    {
        WeightInit();
    }

    public void MakeRoom(Vector2 pos)
    {
        GameObject roomObj = GameManager.instance.objectPoolManager.GetGo("Room");
        Room room = roomObj.GetComponent<Room>();

        roomObj.transform.position = pos;

        room.thisRoom = SetRoom();
        room.Init();
    }

    private void WeightInit()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            total += rooms[i].weight;
        }
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
