
using StarterAssets;
using UnityEngine;

namespace Assets.Scripts.MyProject
{
    public abstract class CharacterControllerBase : MonoBehaviour
    {
        protected float MoveSpeed = 2f;
        protected float RotationSmoothTime = 0.12f;
        protected float JumpHeight = 1.2f;
        protected float SprintSpeed = 5.3f;
        protected float SpeedChangeRate = 10.0f;
        protected bool Grounded = true;

        protected LayerMask GroundLayers;

        protected float Gravity = -15.0f;

        protected virtual void Move() { }
        protected virtual void Rotate() { }
        protected virtual void JumpAndGravity() { }
        protected virtual void GroundedCheck() {}
    }

    public class AnimID
    {
        public int Speed;
        public int Grounded;
        public int Jump;
        public int FreeFall;
        public int MotionSpeed;

        public AnimID()
        {
            Speed = Animator.StringToHash("Speed");
            Grounded = Animator.StringToHash("Grounded");
            Jump = Animator.StringToHash("Jump");
            FreeFall = Animator.StringToHash("FreeFall");
            MotionSpeed = Animator.StringToHash("MotionSpeed");
        }
    }

    public class CharacterMovement
    {
        private StarterAssetsInputs input;
        private CharacterController controller;
        private GameObject mainCamera;
        private bool hasAnimator;
        private Animator animator;
        private AnimID animID;
        private float speed;
        private float animationBlend;
        private float targetRotation;
        private float rotationVelocity;
        private float verticalVelocity;
        private float moveSpeed;
        private float sprintSpeed;
        private float speedChangeRate;
        private float rotationSmoothTime;
        private Transform ownerTransform;

        private bool grounded;
        private float fallTimeoutDelta;
        private float jumpTimeoutDelta;
        private float gravity;
        private float jumpHeight;
        private float jumpTimeout;
        private float fallTimeout;
        private float terminalVelocity = 53.0f;
        private LayerMask groundLayers;

        private float groundedOffset = -0.14f;
        private float groundedRadius = 0.28f;

        public CharacterMovement(
            StarterAssetsInputs input,
            CharacterController controller,
            GameObject mainCamera,
            bool hasAnimator,
            Animator animator,
            AnimID animID,
            float moveSpeed,
            float sprintSpeed,
            float speedChangeRate,
            float rotationSmoothTime,
            Transform ownerTransform,
            float gravity,
            float jumpHeight,
            float jumpTimeout,
            float fallTimeout,
            LayerMask groundLayers)
        {
            this.input = input;
            this.controller = controller;
            this.mainCamera = mainCamera;
            this.hasAnimator = hasAnimator;
            this.animator = animator;
            this.animID = animID;
            this.moveSpeed = moveSpeed;
            this.sprintSpeed = sprintSpeed;
            this.speedChangeRate = speedChangeRate;
            this.rotationSmoothTime = rotationSmoothTime;
            this.ownerTransform = ownerTransform;
            this.gravity = gravity;
            this.jumpHeight = jumpHeight;
            this.jumpTimeout = jumpTimeout;
            this.fallTimeout = fallTimeout;
            this.groundLayers = groundLayers;
        }

        public void Move()
        {
            float targetSpeed = input.sprint ? sprintSpeed : moveSpeed;

            if (input.move == Vector2.zero) targetSpeed = 0.0f;

            float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * speedChangeRate);

                speed = Mathf.Round(speed * 1000f) / 1000f;
            }
            else
            {
                speed = targetSpeed;
            }

            animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
            if (animationBlend < 0.01f) animationBlend = 0f;

            Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

            if (input.move != Vector2.zero)
            {
                targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(ownerTransform.eulerAngles.y, targetRotation, ref rotationVelocity,
                    rotationSmoothTime);

                ownerTransform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

            controller.Move(targetDirection.normalized * (speed * Time.deltaTime) +
                             new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

            if (hasAnimator)
            {
                animator.SetFloat(animID.Speed, animationBlend);
                animator.SetFloat(animID.MotionSpeed, inputMagnitude);
            }
        }

        public void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(ownerTransform.position.x, ownerTransform.position.y - groundedOffset,
                ownerTransform.position.z);

            grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers,
                QueryTriggerInteraction.Ignore);

            if (hasAnimator)
            {
                animator.SetBool(animID.Grounded, grounded);
            }
        }

        public void JumpAndGravity()
        {
            if (grounded)
            {
                fallTimeoutDelta = fallTimeout;

                if (hasAnimator)
                {
                    animator.SetBool(animID.Jump, false);
                    animator.SetBool(animID.FreeFall, false);
                }

                if (verticalVelocity < 0.0f)
                {
                    verticalVelocity = -2f;
                }

                if (input.jump && jumpTimeoutDelta <= 0.0f)
                {
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

                    if (hasAnimator)
                    {
                        animator.SetBool(animID.Jump, true);
                    }
                }

                if (jumpTimeoutDelta >= 0.0f)
                {
                    jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                jumpTimeoutDelta = jumpTimeout;

                if (fallTimeoutDelta >= 0.0f)
                {
                    fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    if (hasAnimator)
                    {
                        animator.SetBool(animID.FreeFall, true);
                    }
                }

                input.jump = false;
            }

            if (verticalVelocity < terminalVelocity)
            {
                verticalVelocity += gravity * Time.deltaTime;
            }
        }
    }

    public class CameraController
    {
        private float topClamp = 70.0f;
        private float bottomClamp = -30.0f;
        private float cameraAngleOverride = 0.0f;

        private GameObject cinemachineCameraTarget;
        private bool lockCameraPosition = false;
        private float cinemachineTargetYaw;
        private float cinemachineTargetPitch;

        private StarterAssetsInputs input;

        private const float threshold = 0.01f;

        public CameraController(
            StarterAssetsInputs input,
            float topClamp,
            float BottomClamp,
            float CameraAngleOverride,
            GameObject CinemachineCameraTarget,
            bool LockCameraPosition)
        {
            this.input = input;
            this.topClamp = topClamp;
            this.bottomClamp = BottomClamp;
            this.cameraAngleOverride = CameraAngleOverride;
            this.cinemachineCameraTarget = CinemachineCameraTarget;
            this.lockCameraPosition = LockCameraPosition;
        }

        public void CameraRotation()
        {
            if (input.look.sqrMagnitude >= threshold && !lockCameraPosition)
            {
                float deltaTimeMultiplier = 1.0f;

                cinemachineTargetYaw += input.look.x * deltaTimeMultiplier;
                cinemachineTargetPitch += input.look.y * deltaTimeMultiplier;
            }

            cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
            cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

            cinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + cameraAngleOverride,
                cinemachineTargetYaw, 0.0f);
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}
