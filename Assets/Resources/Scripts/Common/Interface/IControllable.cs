using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IControllable
{
    public GameObject ParentObj { get; set; }
    event Action<Vector2> moveInput;
    event Action<int> attackInput;
    event Action jumpInput;
    event Action interaction;
}
