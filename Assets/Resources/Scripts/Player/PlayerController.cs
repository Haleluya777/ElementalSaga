using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDataInitializable
{
    private enum UnitState { Idle, Attacking, Moving }

    private Animator anim;
    private IMovable movement;
    private IAttackable attack;
    [SerializeField] private UnitState curState;

    [SerializeField] private GameObject parentObj;
    private Vector2 targetVector; //플레이어 목표 방향.
    private bool isAirial;

    public void DataInit()
    {
        movement = parentObj.GetComponentInChildren<IMovable>();
        attack = parentObj.GetComponentInChildren<IAttackable>();
        anim = parentObj.GetComponent<Animator>();
        isAirial = false;
    }

    void Update()
    {
        PlayerIdle();
        PlayerMove();
        PlayerJump();
        PlayerAttack();
        StateAnimation(curState);
    }

    private void StateAnimation(UnitState state)
    {
        switch (state)
        {
            case UnitState.Idle:
                anim.CrossFade("Idle", 0f);
                break;

            case UnitState.Moving:
                anim.CrossFade("Run", 0f);
                break;
        }
    }

    private void PlayerIdle()
    {
        if (curState == UnitState.Attacking)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) curState = UnitState.Idle;
            return;
        }
        else
        {
            if (targetVector.x == 0) curState = UnitState.Idle;
            return;
        }
    }

    private void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("할렐루야");
            movement.PerformJump();
        }
    }

    private void PlayerMove()
    {
        if (curState == UnitState.Attacking) return;

        float dirX = Input.GetAxisRaw("Horizontal");
        if (curState == UnitState.Attacking)
        {
            dirX = 0;
        }

        if (dirX != 0) curState = UnitState.Moving;

        targetVector.x = dirX;
        movement.PerformMove(targetVector);
    }

    private void PlayerAttack()
    {
        if (curState == UnitState.Attacking) return;
        int attackNum = (Input.inputString.ToUpper()) switch
        {
            ("Z") => 0,
            ("X") => 1,
            ("C") => 2,
            _ => -1
        };
        if (attackNum != -1)
        {
            if (attack.PerformAttack(attackNum)) curState = UnitState.Attacking;
        }
    }
}
