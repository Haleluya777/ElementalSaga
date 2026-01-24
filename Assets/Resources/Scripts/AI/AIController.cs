using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AIController : MonoBehaviour, IDataInitializable, IControllable
{
    public enum UnitState { Idle, Attacking, Moving }

    [Header("AI의 현재 상태")]
    public UnitState curState;

    [SerializeField] private GameObject parentObj;
    public GameObject ParentObj { get => parentObj; set => value = parentObj; }
    private Rigidbody2D rigid;
    private Animator anim;

    private IMovable movement;
    public IAttackable attack;

    public event Action<Vector2> moveInput;
    public event Action<int> attackInput;
    public event Action jumpInput;
    public event Action interaction;

    [Header("AI모듈")]
    public BehaviorTreeGraph behaviorTree;
    private BehaviorTreeGraph runTimeTree;
    private BTNode root;

    private bool runningBT = false;

    void Update()
    {
        if (runningBT) root.Evaluate(this);
        UpdateUnitState();
    }

    public void CallMoveEvent(Vector2 dir)
    {
        moveInput?.Invoke(dir);
    }

    public void CallAttackEvent()
    {
        attackInput?.Invoke(0);
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

        runTimeTree = behaviorTree.Copy() as BehaviorTreeGraph;
        runTimeTree.blackboard = new BlackBoard();
        root = runTimeTree.rootNode;

        rigid = parentObj.GetComponent<Rigidbody2D>();
        anim = parentObj.GetComponentInChildren<Animator>();
        movement = parentObj.GetComponentInChildren<IMovable>();
        attack = parentObj.GetComponentInChildren<IAttackable>();

        runningBT = true;
    }

    private void UpdateUnitState()
    {
        if (curState == UnitState.Attacking)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                curState = UnitState.Idle;
            }
        }
    }
}
