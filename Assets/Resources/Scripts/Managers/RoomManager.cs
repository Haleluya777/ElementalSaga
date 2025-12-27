using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private List<DoorInfo> rooms = new List<DoorInfo>();
    [SerializeField] private int total;

    void Awake()
    {
        WeightInit();
    }

    public void MakeDoor(Vector2 pos, bool boss = false)
    {
        GameObject doorObj = GameManager.instance.objectPoolManager.GetGo("Room");
        Door door = doorObj.GetComponent<Door>();

        doorObj.transform.position = pos;
        door.thisRoom = boss ? SetDoor(true) : SetDoor();
        door.Init();
    }

    private void WeightInit()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            total += rooms[i].weight;
        }
    }

    private DoorInfo SetDoor(bool boss = false)
    {
        int weight = 0;
        int num = 0;

        num = Random.Range(0, total) + 1;
        for (int i = 0; i < rooms.Count; i++)
        {
            weight += rooms[i].weight;
            if (num <= weight)
            {
                DoorInfo temp = new DoorInfo(rooms[i]);
                return temp;
            }
        }
        return null;
    }
}
