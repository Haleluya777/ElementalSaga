using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RoomType { Battle, EliteBattle, Shop, Heal, Boss, Treasure, Mimic, Puzzle, StartRoom }

[System.Serializable]
public class DoorInfo
{
    public int num;
    public Sprite sprite;
    public int weight;
    public RoomType type;

    public DoorInfo(DoorInfo info)
    {
        this.num = info.num;
        this.sprite = info.sprite;
        this.weight = info.weight;
        this.type = info.type;
    }
}
