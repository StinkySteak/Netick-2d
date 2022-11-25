using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Netick;

public class PlayerInputHandler : NetworkBehaviour
{
    public PlayerController PlayerController;

    public override void NetworkUpdate()
    {
        var input = Sandbox.GetInput<PlayerInput>();

        input.MoveDirection = new (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    public override void NetworkFixedUpdate()
    {
        if (FetchInput(out PlayerInput input))
        {
            PlayerController.SetMove(input.MoveDirection);
        }
    }
}
