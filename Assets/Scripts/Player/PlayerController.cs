using Netick;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Network = Netick.Network;

public class PlayerController : NetworkBehaviour
{
    public NetworkRigidbody2D NetworkedRigidbody2D;
    public Rigidbody2D Rigidbody2D;

    public float MoveSpeed;
    [Networked] Vector2 MoveDirection { get; set; }

    public override void NetworkFixedUpdate()
    {
        Move();
    }

    public void SetMove(Vector2 moveDir)
    {
        MoveDirection = moveDir;
    }

    void Move()
    {
        Rigidbody2D.velocity = MoveDirection * MoveSpeed;
    }
}
