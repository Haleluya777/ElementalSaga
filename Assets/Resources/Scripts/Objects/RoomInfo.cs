using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RoomType { Battle, EliteBattle, Shop, Heal, Boss, Treasure, Mimic, Puzzle }

[System.Serializable]
public class RoomInfo
{
    public int num;
    public Sprite sprite;
    public int weight;
    public RoomType type;

    public RoomInfo(RoomInfo info)
    {
        this.num = info.num;
        this.sprite = info.sprite;
        this.weight = info.weight;
        this.type = info.type;
    }
}
