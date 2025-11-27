using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillChanger : MonoBehaviour, IDataInitializable
{
    private IAttackable attack;
    [SerializeField] private GameObject parentObj;

    public void DataInit()
    {
        attack = parentObj.GetComponentInChildren<IAttackable>();
    }

    /// <summary>
    /// 지정된 인덱스의 액티브 스킬을 새로운 스킬로 교체합니다.
    /// </summary>
    /// <param name="skillIndex">교체할 스킬의 인덱스 (0, 1, 2...)</param>
    /// <param name="newSkill">새로운 스킬 모듈</param>
    public void ChangeActiveSkill(int skillIndex, Skill_Module newSkill)
    {
        if (attack == null || attack.ActiveSkills == null)
        {
            Debug.LogError("Attack component or ActiveSkills list is not initialized.");
            return;
        }

        if (skillIndex < 0 || skillIndex >= attack.ActiveSkills.Count)
        {
            Debug.LogError($"Invalid skill index: {skillIndex}");
            return;
        }

        attack.ActiveSkills[skillIndex] = newSkill;
        Debug.Log($"Skill at index {skillIndex} has been changed to {newSkill.name}.");
    }

    /// <summary>
    /// 액티브 스킬 리스트에 새로운 스킬을 추가합니다.
    /// </summary>
    /// <param name="newSkill">추가할 스킬 모듈</param>
    public void AddActiveSkill(Skill_Module newSkill)
    {
        if (attack == null || attack.ActiveSkills == null)
        {
            Debug.LogError("Attack component or ActiveSkills list is not initialized.");
            return;
        }
        attack.ActiveSkills.Add(newSkill);
        Debug.Log($"Skill {newSkill.name} has been added.");
    }

    /// <summary>
    /// 지정된 인덱스의 액티브 스킬을 제거합니다.
    /// </summary>
    /// <param name="skillIndex">제거할 스킬의 인덱스</param>
    public void RemoveActiveSkill(int skillIndex)
    {
        if (attack == null || attack.ActiveSkills == null)
        {
            Debug.LogError("Attack component or ActiveSkills list is not initialized.");
            return;
        }

        if (skillIndex < 0 || skillIndex >= attack.ActiveSkills.Count)
        {
            Debug.LogError($"Invalid skill index: {skillIndex}");
            return;
        }

        Skill_Module removedSkill = attack.ActiveSkills[skillIndex];
        attack.ActiveSkills.RemoveAt(skillIndex);
        Debug.Log($"Skill {removedSkill.name} at index {skillIndex} has been removed.");
    }
}