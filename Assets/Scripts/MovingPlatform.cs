using Netick;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : NetworkBehaviour
{
    Vector2 NextPos;

    public Vector2 StartPos;
    public Vector2 EndPos;

    [Space]

    public float Speed = 5;
    public float Interval = 5;

    [Space]

    public Rigidbody2D Rigidbody2D;

    [Networked] public Vector2 Velocity { get; private set; }
    [Networked] TickTimer FlipTimer { get; set; }

    public override void NetworkStart()
    {
        if (!Sandbox.IsServer) return;

        transform.position = StartPos;
    }

    Vector2 GetPos()
    {
        if (NextPos == StartPos)
            return EndPos;
        else
            return StartPos;
    }

    public override void NetworkFixedUpdate()
    {
        if (!Sandbox.IsServer) return;

        if (FlipTimer.IsExpired(Sandbox))
        {
            FlipTimer = TickTimer.CreateFromSeconds(Sandbox, Interval);
            NextPos = GetPos();
        }

        var direction = (NextPos - transform.position.ToVector2()).normalized;

        Velocity = direction * Speed;

        Rigidbody2D.velocity = Velocity;
    }
}
