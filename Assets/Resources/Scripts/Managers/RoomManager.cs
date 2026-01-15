using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour, IDataInitializable
{
    [SerializeField] private List<DoorInfo> rooms = new List<DoorInfo>();
    [SerializeField] private int total;

    void Awake()
    {
        //WeightInit();
    }

    public void DataInit()
    {
        WeightInit();
    }

    public void MakeDoor(Vector2 pos, bool boss = false)
    {
        GameObject doorObj = GameManager.instance.objectPoolManager.poolDic["Door"].GetGo("Door");
        Door door = doorObj.GetComponent<Door>();

        doorObj.transform.position = pos;
        door.thisRoom = boss ? SetDoor(true) : SetDoor();
        door.Init();
    }

    public void MakeAmendChest(AmendChest.RelicTier tier, Vector2 pos, List<RelicInfo> confirmedRelics)
    {
        GameObject chestObj = GameManager.instance.objectPoolManager.poolDic["Amend"].GetGo("AmendChest");
        AmendChest chest = chestObj.GetComponent<AmendChest>();

        chestObj.transform.position = pos;
        chest.ChestInit(tier, confirmedRelics);
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
        if (boss)
        {
            Debug.Log("보스방 생성.");
            return new DoorInfo(rooms[4]);
        }

        int weight = 0;
        int num = 0;

        num = Random.Range(0, total) + 1;
        for (int i = 0; i < rooms.Count - 1; i++)
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
