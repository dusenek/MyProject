using StarterAssets;
using System;
using UnityEngine;
using UnityEngine.Windows;

namespace Assets.Scripts.MyProject
{
    public class PlayerController : CharacterControllerBase
    {
        [Header("Jump")]
        public float JumpTimeout = 0.50f;
        public float FallTimeout = 0.15f;

        [Header("Camera")]
        public float TopClamp = 70.0f;
        public float BottomClamp = -30.0f;
        public float CameraAngleOverride = 0.0f;
        public bool LockCameraPosition = false;
        public GameObject CinemachineCameraTarget;

        [Header("Audio")]
        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        private AnimID animID;
        private bool hasAnimator;
        private Animator animator;

        private StarterAssetsInputs input;
        private CharacterController controller;
        private GameObject mainCamera;
        private CharacterMovement characterMovement;
        private CameraController cameraController;

        private void Awake()
        {
            if (mainCamera == null)
            {
                mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
            
            GroundLayers = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Ground"));
        }

        private void Start()
        {
            hasAnimator = TryGetComponent(out animator);
            controller = GetComponent<CharacterController>();
            input = GetComponent<StarterAssetsInputs>();

            animID = new AnimID();
            characterMovement = new CharacterMovement(input, controller, mainCamera, hasAnimator, animator,
            animID, MoveSpeed, SprintSpeed, SpeedChangeRate, RotationSmoothTime, transform, 
            Gravity, JumpHeight, JumpTimeout, FallTimeout, GroundLayers);

            cameraController = new CameraController(input, TopClamp, BottomClamp, CameraAngleOverride, CinemachineCameraTarget, LockCameraPosition);
        }

        private void Update()
        {
            hasAnimator = TryGetComponent(out animator);

            JumpAndGravity();
            GroundedCheck();
            Move();
        }

        private void LateUpdate()
        {
            cameraController.CameraRotation();
        }

        protected override void GroundedCheck()
        {
            characterMovement.GroundedCheck();
        }

        protected override void JumpAndGravity()
        {
            // 점프 기능은 막는다.
            //characterMovement.JumpAndGravity();
        }

        protected override void Move()
        {
            characterMovement.Move();
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = UnityEngine.Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(controller.center), FootstepAudioVolume);
            }
        }
    }
}
