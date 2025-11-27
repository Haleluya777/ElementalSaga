using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    Vector2 Dir { get; }
    public void PerformMove(Vector2 vector);
    public void PerformJump();
}
