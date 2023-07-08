using System;
using Core;
using UnityEngine;

namespace Game.Client.Battle
{
    public class CharacterTest : MonoBehaviour
    {
        [SerializeField] private Transform _viewPointTransform;
        private Animator _animator;
        public bool IsTurnEnabled;
        public bool HasRifle;
        public bool IsAiming;

        private static class InputAxis
        {
            public const string MoveForward = "move_forward";
            public const string MoveSideways = "move_sideways";
            public const string Run = "run";
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _viewPointTransform = Camera.main.transform;
        }

        private float _startAngle;
        private Vector3 _startAngleVector = Vector3.left;
        private Vector3 _prevMoveVector;
        private float _isMovingTimer;

        [SerializeField] private Transform _leftFoot;
        [SerializeField] private Transform _rightFoot;

        
        public float MoveLerp = 3f;
        
        void Update()
        {

            Aim();
            var moveForward = Input.GetAxis(InputAxis.MoveForward);
            var moveSideways = Input.GetAxis(InputAxis.MoveSideways);

            var moveForwardRaw = Input.GetAxisRaw(InputAxis.MoveForward);
            var moveSidewaysRaw = Input.GetAxisRaw(InputAxis.MoveSideways);



            var isRunning = Input.GetAxis(InputAxis.Run) > float.Epsilon;

            if (!moveForwardRaw.IsZero() || !moveSidewaysRaw.IsZero())
            {
                _isMovingTimer = 0.25f;
            }
            else
            {
                _isMovingTimer = Math.Max(0f, _isMovingTimer - Time.unscaledDeltaTime);
            }

            bool isMoving = _isMovingTimer > 0f;

            var viewForwardHorizontal = _viewPointTransform.forward.SetY(0f).normalized;
            var viewRightHorizontal = _viewPointTransform.right.SetY(0f).normalized;

            // DebugDrawer.DrawLine3d(transform.position, transform.position + viewRightHorizontal * moveSidewaysRaw * 5, Color.red, Time.deltaTime);

            // DebugDrawer.DrawLine3d(transform.position, transform.position + viewForwardHorizontal * 5, Color.red, Time.deltaTime);
            // DebugDrawer.DrawLine3d(transform.position, transform.position + viewRightHorizontal * 5, Color.green, Time.deltaTime);

            var moveVector = moveForward * viewForwardHorizontal + moveSideways * viewRightHorizontal;

            
            // DebugDrawer.DrawLine3d(transform.position, transform.position + moveVector * 5, Color.magenta, Time.deltaTime);

            if (moveVector.IsZero())
            {
                moveVector = _prevMoveVector;
            }
            else
            {
               // moveVector = Vector3.Slerp(_prevMoveVector, moveVector, Time.deltaTime * MoveLerp);
                _prevMoveVector = moveVector;
            }


            // IsTurnEnabled = Input.GetKey(KeyCode.Space);

            var move3d = transform.InverseTransformDirection(moveVector);
            moveForward = move3d.z;
            moveSideways = move3d.x;



            // DebugDrawer.DrawLine3d(transform.position, transform.position + move3d * 5, Color.white, Time.deltaTime);

            var moveAngle = Angle.Normalize(Vector2.SignedAngle(move3d.GetXZ(), Vector2.up));
            _animator.SetFloat(CharacterAnimator.Variable.WorldMoveX, moveVector.x);
            _animator.SetFloat(CharacterAnimator.Variable.WorldMoveY, moveVector.z);
           
            
            // DebugDrawer.DrawLine3d(transform.position, transform.position + moveVector * 5f, Color.red, Time.deltaTime);
            // DebugDrawer.DrawLine3d(transform.position, transform.position + transform.forward * 5f, Color.red, Time.deltaTime);
            // DebugDrawer.DrawLine3d(transform.position, transform.position + moveForward * transform.forward * 5f +  moveSideways * transform.right * 5f, Color.green, Time.deltaTime);
            // DebugDrawer.DrawText3d(transform.position, Color.green, $"moveAngle: {moveAngle}\n startAngle : {_startAngle}", Time.deltaTime);

            _animator.SetFloat(CharacterAnimator.Variable.MoveY, moveForward);
            _animator.SetFloat(CharacterAnimator.Variable.MoveX, moveSideways);
            _animator.SetFloat(CharacterAnimator.Variable.MoveSpeed, moveVector.magnitude);
            _animator.SetFloat(CharacterAnimator.Variable.MoveAngle, moveAngle);
            _animator.SetBool(CharacterAnimator.Variable.IsRunning, isRunning);
            _animator.SetBool(CharacterAnimator.Variable.IsMoving, isMoving);
            _animator.SetBool(CharacterAnimator.Variable.IsTurnEnabled, IsTurnEnabled);
            _animator.SetBool(CharacterAnimator.Variable.HasRifle, HasRifle);
            
            
            _animator.SetBool(CharacterAnimator.Variable.IsLeftFootLowerRight, _leftFoot.transform.position.y < _rightFoot.transform.position.y);
            
            
            
        }

        
        Plane plane = new Plane(Vector3.up, 0);
      
        private void Aim()
        {
            IsAiming = Input.GetKey(KeyCode.Space);
            
            _animator.SetBool(CharacterAnimator.Variable.IsAiming, IsAiming);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (IsAiming && plane.Raycast(ray, out var distance))
            {
                var aimPosition = ray.GetPoint(distance);
                var aimDirWorld =  (aimPosition - transform.position);
                
                var aimDirLocal = transform.InverseTransformDirection(aimDirWorld);
                
                var aimAngle = Angle.Normalize(Vector2.SignedAngle(aimDirLocal.GetXZ(), Vector2.up));
                _animator.SetFloat(CharacterAnimator.Variable.AimAngle, aimAngle);
                DebugDrawer.DrawLine3d(aimPosition, transform.position, Color.green, Time.deltaTime);
            }
        }


        private void MovementWithStrafe()
        {


        }

        private void MovementWithTurn()
        {
            var moveForward = Input.GetAxis(InputAxis.MoveForward);
            var moveSideways = Input.GetAxis(InputAxis.MoveSideways);



        }
    }

}