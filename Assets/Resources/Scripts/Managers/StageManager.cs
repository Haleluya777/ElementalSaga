using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    //[SerializeField] private List<StageEnemyInfo> enemyList = new List<StageEnemyInfo>();
    [SerializeField] private List<int> enemyList = new List<int>();
    [SerializeField] private List<GameObject> roomObj = new List<GameObject>();
    public int currentRoomId; //currentRoom의 값은 Door클래스가 정해줌.
    private GameObject currentRoom;
    private RoomInfo currentRoomInfo;
    public int totalCount;

    public void StageStart() //스테이지 시작할 때 등장할 몬스터 리스트 만들기.
    {
        totalCount = enemyList.Count; //총 Enemy수.

        //방 프리팹 중 랜덤으로 하나 생성.
        if (currentRoom != null) //이전에 방을 생성했다면
        {
            currentRoomInfo.ReleaseObject();
        }
        currentRoom = GameManager.instance.objectPoolManager.GetGo(roomObj[currentRoomId].name);
        currentRoomInfo = currentRoom.GetComponent<RoomInfo>();

        currentRoom.SetActive(true);
        currentRoom.transform.position = new Vector2(0, 0);

        //Enemy소환.
        foreach (var enemyId in currentRoomInfo.enemyList)
        {
            //일반 몬스터 소환.
            GameObject enemy = GameManager.instance.objectPoolManager.GetGo("Unit");
            EnemyCharacter enemyCom = enemy.GetComponent<EnemyCharacter>();

            Debug.Log(enemyCom is null);

            enemyCom.id = enemyId;
            enemy.name = enemyCom.name;
            enemy.tag = "Enemy";

            if (enemyCom.Type == UnitType.Enemy)
            {
                //일반 몬스터 스폰 위치에 몬스터 이동.
                enemy.transform.position = currentRoomInfo.enemyPos[Random.Range(0, currentRoomInfo.enemyPos.Count)].position;
            }
            else if (enemyCom.Type == UnitType.Elite)
            {
                //엘리트 몬스터 스폰 위치에 몬스터 이동.
                enemy.transform.position = currentRoomInfo.elitePos[Random.Range(0, currentRoomInfo.elitePos.Count)].position;
            }
        }
    }

    public void GiveAmends()
    {
        //보상 UI어쩌고저쩌고
    }

    public void StageClear(bool boss = false)
    {
        if (boss)
        {
            Debug.Log("보스방 생성");
            GameManager.instance.roomManager.MakeDoor(new Vector2(0, -1), true);
            return;
        }
        //다음 방으로 이동할 문 두 개 생성.
        for (int i = 0; i < 2; i++)
        {
            GameManager.instance.roomManager.MakeDoor(currentRoomInfo.doorPos[i].position);
        }
    }
}
