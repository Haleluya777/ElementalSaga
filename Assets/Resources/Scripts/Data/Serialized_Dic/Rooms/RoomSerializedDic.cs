using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomSerializedDic : ScriptableObject
{
    public abstract GameObject GetRandomRoom(int cnt);
}
