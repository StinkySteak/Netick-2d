using UnityEngine;
using Netick;

namespace Netick.Samples.FPS
{
    public class FPSController : NetworkBehaviour
    {
        [SerializeField]
        private float                _movementSpeed                = 10;
        [SerializeField]
        private float                _sensitivityX                 = 1.6f;
        [SerializeField]
        private float                _sensitivityY                 = -1f;    
        [SerializeField]
        private Transform            _cameraParent;
        private CharacterController  _CC;
        private Vector2              _camAngles;

        // Networked properties
        [Networked] public Vector2   YawPitch                     { get; set; }
   
        public override void NetworkStart()
        {
            _CC = GetComponent<CharacterController>();

            if (IsInputSource)
            {
                var cam                     = Sandbox.FindObjectOfType<Camera>();
                cam.transform.parent        = _cameraParent;
                cam.transform.localPosition = Vector3.zero;
                cam.transform.localRotation = Quaternion.identity;
            }
        }

        public override void OnInputSourceLeft()
        {
            // destroy the player object when its input source (controller player) leaves the game
            Sandbox.Destroy(Object);
        }

        public override void NetworkUpdate()
        {
            if (!IsInputSource || !Sandbox.InputEnabled)
                return;

            Vector2 input = new Vector2(Input.GetAxisRaw("Mouse X") * _sensitivityX, Input.GetAxisRaw("Mouse Y") * _sensitivityY);
            Sandbox.GetInput<FPSInput>().YawPitch += input;

            // we apply the rotation in update too to have smooth camera control
            _camAngles = ClampAngles(_camAngles.x + input.x, _camAngles.y + input.y);
            ApplyRotations(_camAngles);
        }

        public override void NetworkFixedUpdate()
        {
            if (FetchInput(out FPSInput input))
            {                
                MoveAndRotate(input);
            }
        }

        private void MoveAndRotate(FPSInput input)
        {
            // rotation
            // note: the rotation happens through the [OnChanged] callback below 
            YawPitch = ClampAngles(YawPitch.x + input.YawPitch.x, YawPitch.y + input.YawPitch.y);

            // movement direction
            var movement = transform.TransformVector(new Vector3(input.Movement.x, 0, input.Movement.y)) * _movementSpeed;
            movement.y   = 0;

            var gravity  = 15f * Vector3.down;

            // move
            _CC.Move((movement + gravity) * Sandbox.FixedDeltaTime);
        }


        [OnChanged(nameof(YawPitch))]
        private void OnNetCamAnglesChanged(Vector2 previous)
        {
            ApplyRotations(YawPitch);
            _camAngles = YawPitch;
        }

        private void ApplyRotations(Vector2 camAngles)
        {
            // on the player transform, we apply yaw
            transform.rotation = Quaternion.Euler(new Vector3(0, camAngles.x, 0));

            // on the weapon/camera holder, we apply the pitch angle
            _cameraParent.localEulerAngles = new Vector3(camAngles.y, 0, 0);
        }


        private Vector2 ClampAngles(float yaw, float pitch)
        {
            return new Vector2(ClampAngle(yaw, -360, 360), ClampAngle(pitch, -80, 80));
        }

        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }
    }
}

