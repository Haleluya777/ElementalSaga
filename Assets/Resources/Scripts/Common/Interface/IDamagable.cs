using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int dmg, ISkillCaster attacker, GameObject character);
    void AddEffectProcess(StatusEffectBase effect);
    //void StatusEffectProcess(float duration, string statuseffectName);
}
