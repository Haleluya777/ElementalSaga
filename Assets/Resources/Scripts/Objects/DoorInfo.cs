using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DoorType { Battle, EliteBattle, Shop, Heal, Boss, Treasure, Mimic, Puzzle }

[System.Serializable]
public class DoorInfo
{
    public int num;
    public Sprite sprite;
    public int weight;
    public DoorType type;

    public DoorInfo(DoorInfo info)
    {
        this.num = info.num;
        this.sprite = info.sprite;
        this.weight = info.weight;
        this.type = info.type;
    }
}
