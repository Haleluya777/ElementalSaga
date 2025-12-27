using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CoroutineRunner coroutineRunner;
    public ObjectPoolManager objectPoolManager;
    public EventManager eventManager;
    public RoomManager roomManager;
    public StageManager stageManager;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("웨이브 종료");
            for (int i = 0; i < 2; i++)
            {
                roomManager.MakeDoor(new Vector2(-1 + (i * 2), -1));
            }

        }
    }
}
