using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Movement : MonoBehaviour, IMovable, IDataInitializable
{
    [SerializeField] private GameObject parentObj;
    private PlayerController controller;
    private Unit unit;
    private Rigidbody2D rigid;
    private Vector2 dir;
    private Vector2 vel;
    public Vector2 Dir => dir;
    public bool IsMovementLocked { get; set; }

    private float moveSpeed;
    private float jumpForce;
    private int addJumpCount;
    private int curJumpCount;

    private void FixedUpdate()
    {
        if (IsMovementLocked) return;

        //Debug.Log(dir.x);
        CheckingGround();
        if (unit.MoveSpeed == 0) return;

        vel = rigid.velocity;

        vel.x = dir.x * unit.MoveSpeed;
        vel.y = rigid.velocity.y;

        rigid.velocity = vel;
        SetLocalScale((int)vel.x / (int)unit.MoveSpeed);
    }

    public void DataInit()
    {
        rigid = parentObj.GetComponent<Rigidbody2D>();
        unit = parentObj.GetComponent<Unit>();
        curJumpCount = unit.AddJumpCount;
    }

    void OnEnable()
    {
        if (parentObj.GetComponent<PlayableCharacter>().controlState == PlayableCharacter.ControlState.Player)
        {
            controller = parentObj.GetComponentInChildren<PlayerController>();
            controller.moveInput += PerformMove;
            controller.jumpInput += PerformJump;
        }
    }

    void OnDisable()
    {
        if (parentObj.GetComponent<PlayableCharacter>().controlState == PlayableCharacter.ControlState.Player)
        {
            controller.moveInput -= PerformMove;
            controller.jumpInput -= PerformJump;
        }
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
            //Debug.Log("호놀룰루");
            Jump();
        }
    }

    public void Jump()
    {
        rigid.AddForce(Vector2.up * unit.JumpForce, ForceMode2D.Impulse);
    }

    public void PerformMove(Vector2 vector)
    {
        //Debug.Log("이동 호출");
        dir = vector;
    }

    private void SetLocalScale(int dirX)
    {
        if (dirX == 0) return;
        parentObj.transform.localScale = new Vector2(dirX, 1);
    }

    private void CheckingGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(parentObj.transform.position, new Vector2(.5f, .1f), 0, Vector2.down, .1f, 1 << 3);
        if (hit.collider != null)
        {
            //Debug.Log("땅을 딛고 있음");
            unit.isAirial = false;
            curJumpCount = unit.AddJumpCount;
        }
        else if (hit.collider == null)
        {
            //Debug.Log("공중에 있음");
            unit.isAirial = true;
        }

        Debug.DrawRay(parentObj.transform.position, Vector2.down * .1f, Color.red);
    }
}
