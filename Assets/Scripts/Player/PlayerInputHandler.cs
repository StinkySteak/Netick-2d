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

        input.Horizontal = Input.GetAxis("Horizontal");
        input.Jump = Input.GetKey(KeyCode.Space);
        input.PauseTimer = Input.GetKey(KeyCode.X);
        input.UnpauseTimer = Input.GetKey(KeyCode.Y);

    }

    public override void NetworkFixedUpdate()
    {
        if (FetchInput(out PlayerInput input))
        {
            PlayerController.SetMove(input.Horizontal);

            if (input.Jump)
                PlayerController.SetJump();

            if (input.PauseTimer)
                PlayerController.SetTimerPause(true);
            if (input.UnpauseTimer)
                PlayerController.SetTimerPause(false);
        }
    }
}
