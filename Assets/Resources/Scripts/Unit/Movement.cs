using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour, IMovable, IDataInitializable
{
    [SerializeField] private GameObject parentObj;
    private Unit unit;
    private Rigidbody2D rigid;
    private Vector2 dir;
    public Vector2 Dir => dir;

    private float moveSpeed;
    private float jumpForce;
    private int addJumpCount;
    private int curJumpCount;

    private void FixedUpdate()
    {
        CheckingGround();
    }

    public void DataInit()
    {
        rigid = parentObj.GetComponent<Rigidbody2D>();
        unit = parentObj.GetComponent<Unit>();
        curJumpCount = unit.AddJumpCount;
    }

    public void PerformJump()
    {
        if (rigid.velocity.y != 0) //y축 가속도가 0이 아닐 때(지면에 가만히 있을 때.)
        {
            if (curJumpCount > 0)
            {
                Jump();
                curJumpCount--;
            }
            else return;
        }
        else if (rigid.velocity.y == 0)
        {
            Debug.Log("호놀룰루");
            Jump();
        }
    }

    public void Jump()
    {
        rigid.AddForce(Vector2.up * unit.JumpForce, ForceMode2D.Impulse);
    }

    public void PerformMove(Vector2 vector)
    {
        if (unit.MoveSpeed == 0) return;
        dir.x = vector.x * unit.MoveSpeed;
        dir.y = rigid.velocity.y;
        rigid.velocity = dir;
        SetLocalScale((int)dir.x / (int)unit.MoveSpeed);
    }

    private void SetLocalScale(int dirX)
    {
        if (dirX == 0) return;
        parentObj.transform.localScale = new Vector2(dirX, 1);
    }

    private void CheckingGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(parentObj.transform.position, new Vector2(.5f, .05f), 0, Vector2.down, 0, 1 << 3);
        if (hit.collider != null)
        {
            //Debug.Log("닿는 중");
            curJumpCount = unit.AddJumpCount;
        }

        Debug.DrawRay(parentObj.transform.position, Vector2.down * 0, Color.red);
    }
}
