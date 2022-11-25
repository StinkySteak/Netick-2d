using UnityEngine;
using Netick;

namespace Netick.Samples.FPS
{
    public class FPSInput : NetworkInput
    {
        public Vector2 YawPitch;
        public Vector2 Movement;
        public bool    ShootInput;
    }
}