using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYellowpaper.SerializedCollections
{
    [CreateAssetMenu(fileName = "BossRooms", menuName = "ScriptableObject/SerializedDic/BossRooms")]
    [System.Serializable]
    public class CombatRooms : RoomSerializedDic
    {
        [SerializedDictionary("Num", "RoomObj")]
        public SerializedDictionary<int, GameObject> room;
        private List<GameObject> rooms = new List<GameObject>();

        public override GameObject GetRandomRoom(int cnt)
        {
            GameObject randomRoom;

            foreach (var _room in room)
            {
                rooms.Add(_room.Value);
            }
            randomRoom = rooms[Random.Range(0, rooms.Count)];
            rooms.Clear();

            return randomRoom;
        }
    }
}