using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float SmoothTime = 0.1f;

    public Vector3 Offset = new(0, 0, -10);

    private void LateUpdate()
    {
        if (PlayerSetup.LocalPlayer == null)
            return;

        Vector3 nextPosition = PlayerSetup.LocalPlayer.transform.position;
        nextPosition.z = 0;

        nextPosition += Offset;


        var finalPos = Vector3.Lerp(transform.position, nextPosition, Time.deltaTime * SmoothTime);

        transform.position = finalPos;
    }
}
