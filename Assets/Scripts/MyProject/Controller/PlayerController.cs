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

        private AnimID _animID;
        private bool _hasAnimator;
        private Animator _animator;

        private StarterAssetsInputs _input;
        private CharacterController _controller;
        private GameObject _mainCamera;
        private CharacterMovement _characterMovement;
        private CameraController cameraController;

        private void Awake()
        {
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
            
            GroundLayers = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Ground"));
        }

        private void Start()
        {
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();

            _animID = new AnimID();
            _characterMovement = new CharacterMovement(_input, _controller, _mainCamera, _hasAnimator, _animator,
            _animID, MoveSpeed, SprintSpeed, SpeedChangeRate, RotationSmoothTime, transform, 
            Gravity, JumpHeight, JumpTimeout, FallTimeout, GroundLayers);

            cameraController = new CameraController(_input, TopClamp, BottomClamp, CameraAngleOverride, CinemachineCameraTarget, LockCameraPosition);
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

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
            _characterMovement.GroundedCheck();
        }

        protected override void JumpAndGravity()
        {
            // 점프 기능은 막는다.
            //_characterMovement.JumpAndGravity();
        }

        protected override void Move()
        {
            _characterMovement.Move();
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = UnityEngine.Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }
}
