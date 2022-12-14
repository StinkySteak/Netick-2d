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

    public float JumpCooldown = 2;
    public float JumpForce = 5;

    public Transform RaycastPoint;

    public float RaycastDistance = 0.1f;

    [Networked] bool IsGrounded { get; set; }
    [Networked] float Horizontal { get; set; }

    [Networked] public TickTimer JumpCooldownTimer { get; set; }

    [Networked] public PauseableTickTimer PauseableTimer { get; set; }

    public override void NetworkStart()
    {
        PauseableTimer = PauseableTickTimer.CreateFromSeconds(Sandbox, 10);
    }

    public override void NetworkFixedUpdate()
    {
        SetGrounded();
        Move();
        PredictPlatform();
    }
    public void SetTimerPause(bool value)
    {
        if (value)
            PauseableTimer = PauseableTimer.Pause(Sandbox);
        else
            PauseableTimer = PauseableTimer.Resume(Sandbox);
    }
    /// <summary>
    /// Client Side Prediction for Moving Platform
    /// </summary>
    void PredictPlatform()
    {
        var hit = Physics2D.Raycast(RaycastPoint.transform.position, Vector2.down, RaycastDistance);

        if (hit)
        {
            if (hit.collider.transform.parent == null)
                return;

            if (!hit.collider.transform.parent.TryGetComponent(out MovingPlatform platform))
                return;

            // Getting The Predicted Velocity
            var predictedVelocity = platform.Rigidbody2D.velocity;

            Rigidbody2D.velocity += predictedVelocity;
        }
    }

    void SetGrounded()
    {
        IsGrounded = Physics2D.Raycast(RaycastPoint.position, Vector2.down, RaycastDistance);
    }

    public void SetJump()
    {
        if (!IsGrounded)
            return;

        if (!JumpCooldownTimer.IsExpired(Sandbox))
            return;

        JumpCooldownTimer = TickTimer.CreateFromSeconds(Sandbox, JumpCooldown);
        Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, JumpForce);
    }

    public void SetMove(float horizontal)
    {
        Horizontal = horizontal;
    }

    void Move()
    {
        Rigidbody2D.velocity = new Vector2(Horizontal * MoveSpeed * Sandbox.FixedDeltaTime, Rigidbody2D.velocity.y);
    }
}
