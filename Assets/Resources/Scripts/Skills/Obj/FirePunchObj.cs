using UnityEngine;

public class FirePunchObj : SkillObjBase
{
    private ISkillCaster caster;
    private Rigidbody2D rigid;
    public int dmg;
    public int stunDmg;
    private Vector2 dir;
    private const int OBJECT_SPEED = 3;

    private void Start()
    {
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

    public override void ObjInit(Vector2 _dir, int _dmg, int _stunDmg, string _tag, ISkillCaster _caster)
    {
        rigid = this.GetComponent<Rigidbody2D>();
        dir = _dir;
        dmg = _dmg;
        stunDmg = _stunDmg;
        gameObject.tag = _tag;
        caster = _caster;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.gameObject.tag != other.gameObject.tag)
        {
            var damageable = other.GetComponentInChildren<IDamageable>();
            damageable.TakeDamage(dmg, stunDmg, caster, other.gameObject);
        }
    }
}
