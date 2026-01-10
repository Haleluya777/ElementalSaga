using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public DialogueRunner dialogueRunner;
    public CoroutineRunner coroutineRunner;
    public ObjectPoolManager objectPoolManager;
    public EventManager eventManager;
    public RoomManager roomManager;
    public StageManager stageManager;
    public CanvasManager canvasManager;
    public UnitManager unitManager; //현재 조종 중인 현재 플레이어 유닛에 관련된 매니저.

    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }

        foreach (var init in GetComponentsInChildren<IDataInitializable>())
        {
            init.DataInit();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("웨이브 종료");
            for (int i = 0; i < 2; i++)
            {
                //stageManager.GiveAmends();
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            dialogueRunner.ProccessNextLine();
        }
    }
}
