using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformAttack : BTNode
{
    public int SkillNum;
    private Skill_Module skill;
    private bool isExecuting = false;
    private int lastFrameVisited = -1;

    public void SkillModuleSetting(AIController controller)
    {
        if (skill == null) skill = controller.attack.ActiveSkills[SkillNum];
    }

    public override NodeState Evaluate(AIController controller)
    {
        // 노드에 새로 진입했거나 실행이 끊겼었다면 상태 초기화
        if (Time.frameCount != lastFrameVisited + 1)
        {
            isExecuting = false;
        }
        lastFrameVisited = Time.frameCount;

        SkillModuleSetting(controller);

        if (blackboard is null) return NodeState.Failure;
        if (skill == null) return NodeState.Failure;

        if (isExecuting)
        {
            if (controller.curState != AIController.UnitState.Attacking)
            {
                isExecuting = false;
                Debug.Log($"[PerformAttack] 공격 종료 감지 -> Success 반환 (SkillNum: {SkillNum})");
                return NodeState.Success;
            }
            return NodeState.Running;
        }

        if (skill.OnCoolDown) return NodeState.Failure;
        if (controller.curState == AIController.UnitState.Attacking) return NodeState.Running;

        controller.curState = AIController.UnitState.Attacking;

        controller.attack.PerformSkill(skill);
        isExecuting = true;

        return NodeState.Running;
    }

    public override bool CanExecute(AIController controller) //노드가 실행 가능한 상태인지 체크 True = 실행가능 False = 실행 불가능.
    {
        SkillModuleSetting(controller);
        if (skill == null) return false;
        return !skill.OnCoolDown;
    }
}
