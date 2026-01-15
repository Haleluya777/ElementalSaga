using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AIController : MonoBehaviour, IDataInitializable, IControllable
{
    [SerializeField] private GameObject parentObj;
    private Rigidbody2D rigid;
    public float timer;

    public event Action<Vector2> moveInput;
    public event Action<int> attackInput;
    public event Action jumpInput;
    public event Action interaction;

    [Header("AI모듈")]
    public BehaviorTreeGraph behaviorTree;
    private BehaviorTreeGraph runTimeTree;
    private BTNode root;

    void Awake()
    {
        runTimeTree = Instantiate(behaviorTree);
        rigid = parentObj.GetComponent<Rigidbody2D>();
        root = runTimeTree.rootNode;
    }

    // Update is called once per frame
    void Update()
    {
        root.Evaluate();
    }

    public void DataInit()
    {
        // if (parentObj.GetComponent<PlayableCharacter>().controlState == PlayableCharacter.ControlState.Player)
        // {
        //     playerInput.onActionTriggered -= ActionTrigger;
        //     playerInput.onActionTriggered += ActionTrigger;
        // }

        // movement = parentObj.GetComponentInChildren<IMovable>();
        // attack = parentObj.GetComponentInChildren<IAttackable>();
        // anim = parentObj.GetComponent<Animator>();
        // isAirial = false;
    }
}
