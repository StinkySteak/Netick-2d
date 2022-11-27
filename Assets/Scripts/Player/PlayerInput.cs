using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Netick;

public class PlayerInput : NetworkInput
{
    public float Horizontal;
    public bool Jump;

    public bool PauseTimer;
    public bool UnpauseTimer;
}
