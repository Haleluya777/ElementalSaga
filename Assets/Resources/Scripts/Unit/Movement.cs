using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Movement : MonoBehaviour, IMovable, IDataInitializable
{
    [SerializeField] private GameObject parentObj;
    private IControllable controller;
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

    private bool _jumpRequested;

    private void FixedUpdate()
    {
        CheckingGround();

        if (_jumpRequested)
        {
            HandleJump();
            _jumpRequested = false;
        }

        if (IsMovementLocked) return;

        if (unit.MoveSpeed == 0) return;

        vel = rigid.velocity;

        vel.x = dir.x * unit.MoveSpeed;
        // y 속도는 점프와 중력에 의해 제어되므로 여기서는 x만 변경합니다.
        // vel.y = rigid.velocity.y; 

        rigid.velocity = vel;
        SetLocalScale((int)vel.x / (int)unit.MoveSpeed);
    }

    public void DataInit()
    {
        rigid = parentObj.GetComponent<Rigidbody2D>();
        unit = parentObj.GetComponent<Unit>();
        curJumpCount = unit.AddJumpCount;
        _jumpRequested = false;
        controller = parentObj.GetComponentInChildren<IControllable>();

        if (controller is not null)
        {
            controller.moveInput += PerformMove;
            controller.jumpInput += PerformJump;
        }
    }

    void OnDisable()
    {
        controller.moveInput -= PerformMove;
        controller.jumpInput -= PerformJump;
    }

    // 점프 '요청'만 기록합니다.
    public void PerformJump()
    {
        _jumpRequested = true;
    }

    // 실제 점프 로직
    private void HandleJump()
    {
        // 지상 점프
        if (!unit.isAirial)
        {
            Jump();
        }
        // 공중 점프
        else if (curJumpCount > 0)
        {
            Jump();
            curJumpCount--;
        }
    }

    public void Jump()
    {
        // 점프력을 더하기 전에 y축 속도를 0으로 만들어, 항상 일정한 힘으로 점프하도록 합니다.
        // (특히 떨어지는 중에 점프할 때 일관된 느낌을 줍니다.)
        rigid.velocity = new Vector2(rigid.velocity.x, 0);
        rigid.AddForce(Vector2.up * unit.JumpForce, ForceMode2D.Impulse);
    }

    public void PerformMove(Vector2 vector)
    {
        dir = vector;
    }

    private void SetLocalScale(int dirX)
    {
        if (dirX == 0) return;
        parentObj.transform.localScale = new Vector2(dirX, 1);
    }

    private void CheckingGround()
    {
        // 지상 판정을 위한 박스캐스트
        RaycastHit2D hit = Physics2D.BoxCast(parentObj.transform.position, new Vector2(.5f, .1f), 0, Vector2.down, 1f, 1 << 3);
        if (hit.collider != null)
        {
            if (unit.isAirial) // 공중에 있다가 처음 땅에 닿는 순간
            {
                unit.isAirial = false;
                curJumpCount = unit.AddJumpCount; // 점프 횟수 초기화
            }
        }
        else
        {
            if (!unit.isAirial) // 땅에 있다가 처음 공중으로 뜨는 순간
            {
                unit.isAirial = true;
            }
        }

        Debug.DrawRay(parentObj.transform.position, Vector2.down * .1f, Color.red);
    }
}
