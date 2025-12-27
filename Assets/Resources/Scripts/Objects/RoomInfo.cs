using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : PoolAble
{
    public List<int> enemyList = new List<int>();
    public List<Transform> enemyPos = new List<Transform>();
    public List<Transform> elitePos = new List<Transform>();
    public Transform[] doorPos = new Transform[2];
}
