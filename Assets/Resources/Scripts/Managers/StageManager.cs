using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    //[SerializeField] private List<StageEnemyInfo> enemyList = new List<StageEnemyInfo>();
    [SerializeField] private List<int> enemyList = new List<int>();
    [SerializeField] private List<GameObject> roomObj = new List<GameObject>(); //방들 중 하나를 랜덤으로 가져올 리스트(삭제 예정)
    [SerializeField] private List<RoomSerializedDic> roomMaps = new List<RoomSerializedDic>();
    public int stage; //현재 스테이지 번호.

    public int currentRoomId; //currentRoom의 값은 Door클래스가 정해줌.
    [SerializeField] private GameObject currentRoom; //현재 플레이어가 위치한 방 오브젝트
    [SerializeField] private RoomInfo currentRoomInfo; //현재 플레이어가 위치한 방의 방 정보 클래스

    private int previousTotalCound;
    public int totalCount;
    public int amendCount; //줄 보상의 개수
    private List<Transform> pos = new List<Transform>();
    private int wave = 2;

    void Start()
    {
        StageStart();
    }

    void FixedUpdate()
    {
        //몬스터의 수가 하나씩 줄어들 때마다 체크.
        if (previousTotalCound != totalCount)
        {
            if (totalCount == 0)
            {
                if (wave != 0) //남은 웨이브가 존재할 때.
                {
                    Debug.Log("몬스터 리젠");
                    SettingEnemy();
                    wave--;
                }

                if (wave == 0) //남은 웨이브가 없을 때
                {
                    //현재 스테이지 상황에 따라서 스테이지 클리어.
                    if (stage > 9) StageClear(true);
                    if (stage <= 8) StageClear();
                }
            }
            //몬스터 수 초기화
            previousTotalCound = totalCount;
        }
    }

    public void StageStart(RoomType roomType = RoomType.StartRoom) //스테이지 시작할 때 등장할 몬스터 리스트 만들기.
    {
        if (roomType == RoomType.StartRoom)
        {
            Debug.Log("시작방임");
            //StageClear();
            return;
        }

        //방 프리팹 중 랜덤으로 하나 생성.
        if (currentRoom != null) //이전에 방을 생성했다면
        {
            if (currentRoom.name == "StartRoom") currentRoom.SetActive(false);
            else currentRoomInfo.ReleaseObject();
        }

        //이 부분 수정 필요
        currentRoom = LocalGameManager.instance.objectPoolManager.poolDic["Rooms"].GetGo("CombatRoom_1");
        //여기까지

        //수정본
        //CreateRoom(roomType);
        currentRoomInfo = currentRoom.GetComponent<RoomInfo>();
        currentRoomInfo.type = roomType;


        //방 오브젝트 활성화 및 위치 조정.
        currentRoom.SetActive(true);
        currentRoom.transform.position = new Vector2(0, 0);

        //방의 타입에 따른 메서드 실행
        switch (roomType)
        {
            case RoomType.Shop:
                currentRoomInfo.SetShopRelics();
                break;

            default: //전투가 필요한 방일 때.
                SettingEnemy();
                break;
        }
    }

    private void SettingEnemy()
    {
        totalCount = currentRoomInfo.enemyList.Count; //총 Enemy수.
        previousTotalCound = totalCount;

        // 중복 소환 방지를 위한 가용 위치 리스트 복사
        List<Transform> availableEnemyPos = new List<Transform>(currentRoomInfo.enemyPos);
        List<Transform> availableElitePos = new List<Transform>(currentRoomInfo.elitePos);

        //Enemy소환.
        foreach (var enemyName in currentRoomInfo.enemyList)
        {
            //일반 몬스터 소환.
            GameObject enemy = LocalGameManager.instance.objectPoolManager.poolDic["Units"].GetGo(enemyName);
            EnemyCharacter enemyCom = enemy.GetComponent<EnemyCharacter>();
            //Debug.Log(enemyCom is null);

            enemy.tag = "Enemy";

            if (enemyCom.Type == UnitType.Enemy)
            {
                //일반 몬스터 스폰 위치에 몬스터 이동.
                if (availableEnemyPos.Count > 0)
                {
                    int randIdx = Random.Range(0, availableEnemyPos.Count);
                    enemy.transform.position = availableEnemyPos[randIdx].position;
                    availableEnemyPos.RemoveAt(randIdx);
                }
                else
                {
                    Debug.LogWarning("일반 몬스터 스폰 위치가 부족합니다.");
                }
            }
            else if (enemyCom.Type == UnitType.Elite || enemyCom.Type == UnitType.Boss)
            {
                //엘리트 몬스터 스폰 위치에 몬스터 이동.
                if (availableElitePos.Count > 0)
                {
                    int randIdx = Random.Range(0, availableElitePos.Count);
                    enemy.transform.position = availableElitePos[randIdx].position;
                    availableElitePos.RemoveAt(randIdx);
                }
                else
                {
                    Debug.LogWarning("엘리트/보스 몬스터 스폰 위치가 부족합니다.");
                }
            }
        }
        pos.Clear();
    }

    public void StageClear(bool boss = false)
    {
        Debug.Log("스테이지 클리어!");
        if (totalCount > 0) return;

        if (boss)
        {
            wave = 0;
            Debug.Log("보스방 생성");
            //GameManager.instance.roomManager.MakeDoor(new Vector2(0, -1), true);
            return;
        }
        //다음 방으로 이동할 문 두 개 생성.
        for (int i = 0; i < 2; i++)
        {
            //GameManager.instance.roomManager.MakeDoor(currentRoomInfo.doorPos[i].position);
        }

        wave = 2;
        //보상 상자 생성.
        //GameManager.instance.roomManager.MakeAmendChest(currentRoomInfo.SetChestTier(), new Vector2(0, -4), currentRoomInfo.confirmedRelics);
    }

    private GameObject CreateRoom(RoomType type)
    {
        switch (type)
        {

        }
        return null;
    }
}
