using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviour, IDataInitializable
{
    private enum UnitState { Idle, Attacking, Moving }

    [SerializeField] private PlayerInput playerInput;
    public Action<Vector2> moveInput;
    public Action<int> attackInput;
    public Action jumpInput;
    public Action interaction;

    private Vector2 moveX;
    private Coroutine continuousSkill;
    private Animator anim;
    private IMovable movement;
    private IAttackable attack;

    [SerializeField] private UnitState curState;
    [SerializeField] private GameObject parentObj;

    private Vector2 targetVector; //플레이어 목표 방향.
    private bool isAirial;
    public bool ModifierAtt;

    public void DataInit()
    {
        if (parentObj.GetComponent<PlayableCharacter>().controlState == PlayableCharacter.ControlState.Player)
        {
            playerInput.onActionTriggered -= ActionTrigger;
            playerInput.onActionTriggered += ActionTrigger;
        }

        movement = parentObj.GetComponentInChildren<IMovable>();
        attack = parentObj.GetComponentInChildren<IAttackable>();
        anim = parentObj.GetComponent<Animator>();
        isAirial = false;
    }

    void OnDisable()
    {
        if (parentObj.GetComponent<PlayableCharacter>().controlState == PlayableCharacter.ControlState.Player)
        {
            playerInput.onActionTriggered -= ActionTrigger;
        }
    }

    void Update()
    {
        StateActions(curState);
        StateAnimation(curState);
        Debug.DrawRay(transform.position, Vector2.down * .7f, Color.red, .2f);
    }

    void LateUpdate()
    {
        // LateUpdate에서 상태를 관리하여 타이밍 문제를 해결합니다.
        PlayerIdle();
        //ModifierAtt = false;
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
                // 입력 값을 항상 moveX에 저장합니다.
                moveX = context.ReadValue<Vector2>();
                // 공격 중이 아닐 때만 moveInput 이벤트를 호출합니다.
                if (curState != UnitState.Attacking)
                {
                    moveInput?.Invoke(moveX);
                }
                break;

            case "ModifierAtt":
                ModifierAtt = context.performed;
                break;


            case "Attack":
                if (context.control is KeyControl keyControl)
                {
                    if (Enum.TryParse<KeyCode>(keyControl.displayName, true, out var pressedKey))
                    {
                        if (context.performed)
                        {
                            SkillKeyDown(pressedKey, ModifierAtt);
                        }
                        else if (context.canceled)
                        {
                            SkillKeyUp(pressedKey, ModifierAtt);
                        }
                    }
                }
                break;

            case "Dash":
                break;

            case "Jump":
                if (context.performed) jumpInput?.Invoke();
                break;

            case "Interaction":
                if (context.performed)
                {
                    interaction?.Invoke();
                    Debug.Log("상호작용 시도");
                }
                break;
        }
    }

    private void PlayerIdle()
    {
        if (curState == UnitState.Attacking)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                curState = UnitState.Idle;
                moveInput?.Invoke(moveX);
            }
        }
        else // 공격 중이 아닐 때
        {
            if (moveX.x != 0)
            {
                curState = UnitState.Moving;
            }
            else
            {
                curState = UnitState.Idle;
            }
        }
    }

    private void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            movement.PerformJump();
        }
    }

    private void StateActions(UnitState state)
    {
        switch (state)
        {
            case UnitState.Attacking:
                // parentObj.transform.GetChild(1).gameObject.SetActive(false);
                break;
            case UnitState.Moving:
                // parentObj.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case UnitState.Idle:
                // parentObj.transform.GetChild(1).gameObject.SetActive(true);
                break;
        }
    }

    private void SkillKeyDown(KeyCode keyCode, bool modifier)
    {
        if (curState == UnitState.Attacking) return;

        int attNum = GetAttNum(keyCode);
        if (attNum == -1) return;

        if (attack == null || attNum >= attack.ActiveSkills.Count) return;
        var skill = !modifier ? attack.ActiveSkills[attNum] : attack.ModifiedActiveSkills[attNum];
        //Debug.Log(skill.name);

        if (skill.activeType == Skill_Module.ActiveType.OnDown) //단발성 스킬
        {
            if (attack.PerformAttack(skill))
            {
                Debug.Log("할렐루야");
                curState = UnitState.Attacking;
                moveInput?.Invoke(Vector2.zero);
            }
            else return;
        }
        else //지속성 스킬
        {
            if (attack.PerformAttack(skill))
            {
                curState = UnitState.Attacking;
                moveInput?.Invoke(Vector2.zero);
                if (continuousSkill != null) StopCoroutine(continuousSkill);
                continuousSkill = StartCoroutine(SkillRoutine(skill, attNum));
            }
        }
    }

    private void SkillKeyUp(KeyCode keyCode, bool modifier)
    {
        int attNum = GetAttNum(keyCode);
        if (attNum == -1) return;

        if (attack == null || attNum >= attack.ActiveSkills.Count) return;
        var skill = !modifier ? attack.ActiveSkills[attNum] : attack.ModifiedActiveSkills[attNum];

        if (skill.activeType != Skill_Module.ActiveType.OnDown)
        {
            if (continuousSkill != null)
            {
                StopCoroutine(continuousSkill);
                continuousSkill = null;
            }
        }
    }

    private int GetAttNum(KeyCode keyCode)
    {
        int attNum = (keyCode) switch
        {
            KeyCode.Z => 0,
            KeyCode.X => 1,
            KeyCode.C => 2,
            KeyCode.V => 3,
            _ => -1
        };
        return attNum;
    }

    private IEnumerator SkillRoutine(Skill_Module skill, int skillNum)
    {
        while (true)
        {
            if (attack.PerformAttack(skill))
            {
                curState = UnitState.Attacking;
                yield return null;
                yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
                curState = UnitState.Idle;
            }
            yield return new WaitForSeconds(.1f);
        }
    }
}