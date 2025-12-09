using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviour, IDataInitializable
{
    private enum UnitState { Idle, Attacking, Moving }

    [SerializeField] private PlayerInput playerInput;
    public Action<Vector2> moveInput;
    public Action<int> attackInput;

    private Vector2 moveX;
    private Coroutine continuousSkill;
    private Animator anim;
    private IMovable movement;
    private IAttackable attack;
    [SerializeField] private UnitState curState;

    [SerializeField] private GameObject parentObj;
    private Vector2 targetVector; //플레이어 목표 방향.
    private bool isAirial;

    public void DataInit()
    {
        if (parentObj.GetComponent<PlayableCharacter>().controlState == PlayableCharacter.ControlState.Player)
        {
            playerInput.onActionTriggered += ActionTrigger;
        }

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

    private void ActionTrigger(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Movement":
                moveX = context.ReadValue<Vector2>();
                break;

            case "Attack":
                if (context.control is KeyControl keyControl)
                {
                    Key pressedKey = keyControl.keyCode;
                    Debug.Log(pressedKey);
                }
                break;

            case "Jump":
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
        if (curState == UnitState.Attacking)
        {
            parentObj.transform.GetChild(1).gameObject.SetActive(false);
            return;
        }
        else
        {
            parentObj.transform.GetChild(1).gameObject.SetActive(true);
            moveInput?.Invoke(moveX);
        }

        if (moveX.x != 0) curState = UnitState.Moving;
    }

    private void PlayerAttack()
    {
        ProccessSkillInput(KeyCode.Z, 0);
        ProccessSkillInput(KeyCode.X, 1);
        ProccessSkillInput(KeyCode.C, 2);
    }

    private void ProccessSkillInput(KeyCode keyCode, int attNum)
    {
        if (attack == null || attNum >= attack.ActiveSkills.Count) return;
        var skill = attack.ActiveSkills[attNum];
        //Debug.Log("입력함");

        // 지속 스킬의 경우, 키를 뗄 때 코루틴을 중지 (상태와 무관하게 항상 체크)
        if (skill.activeType != Skill_Module.ActiveType.OnDown && Input.GetKeyUp(keyCode))
        {
            if (continuousSkill != null)
            {
                StopCoroutine(continuousSkill);
                continuousSkill = null;
            }
        }

        // 키를 누르는 로직은 공격 중이 아닐 때만 실행
        if (Input.GetKeyDown(keyCode))
        {
            if (curState == UnitState.Attacking) return;

            if (skill.activeType == Skill_Module.ActiveType.OnDown) //단발성 스킬
            {
                if (attack.PerformAttack(attNum)) curState = UnitState.Attacking;
            }
            else //지속성 스킬
            {
                if (attack.PerformAttack(attNum))
                {
                    curState = UnitState.Attacking;
                    if (continuousSkill != null) StopCoroutine(continuousSkill);
                    continuousSkill = StartCoroutine(SkillRoutine(skill, attNum));
                }
            }
        }
    }

    private IEnumerator SkillRoutine(Skill_Module skill, int skillNum)
    {
        while (true)
        {
            if (attack.PerformAttack(skillNum))
            {
                curState = UnitState.Attacking;
                yield return null; //애니메이션 상태가 바뀔 때까지 한 프레임 대기.
                yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
                curState = UnitState.Idle;
            }
            yield return new WaitForSeconds(.1f);
        }
    }
}
