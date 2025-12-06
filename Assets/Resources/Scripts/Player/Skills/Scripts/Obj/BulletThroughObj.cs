using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletThroughObj : SkillObjBase
{
    private ISkillCaster caster;
    private Rigidbody2D rigid;
    public int dmg;
    private Vector2 dir;
    private const int OBJECT_SPEED = 10;
    private bool reinforced;

    private void Start()
    {
        if (reinforced)
        {
            Debug.Log("강화된 탄환 발사!");
        }
        transform.localScale = new Vector3(this.transform.localScale.x * dir.x, this.transform.localScale.y, 0);
        //임시
        Destroy(this.gameObject, 2f);
    }

    private void FixedUpdate()
    {
        ObjMovement();
    }

    public override void ObjMovement()
    {
        rigid.velocity = Vector2.right * dir.x * OBJECT_SPEED;
    }

    public override void ObjInit(Vector2 _dir, int _dmg, string _tag, ISkillCaster _caster, bool _reinforced)
    {
        rigid = this.GetComponent<Rigidbody2D>();
        dir = _dir;
        dmg = _dmg;
        gameObject.tag = _tag;
        caster = _caster;
        reinforced = _reinforced;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<IDamageable>(out var damageable)) return;
        if (this.gameObject.tag != other.gameObject.tag)
        {
            damageable = other.GetComponentInChildren<IDamageable>();
            damageable.TakeDamage(dmg, caster, other.gameObject);
            if (reinforced)
            {
                other.TryGetComponent<Unit>(out var enemyUnit);
                enemyUnit.AddEffectProcess(new Debuff_Stun(.5f, other.GetComponent<Unit>(), "Stun", caster.GetGameObject()));
                enemyUnit.AddEffectProcess(new DeBuff_LatedDamage(.5f, other.GetComponent<Unit>(), "LatedDamage", caster.GetGameObject(), dmg));
            }
        }
    }
}
