using UnityEngine;

namespace Game.Client.Battle
{
    public static partial class CharacterAnimator
    {
        public static class Variable
        {
            public static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
            public static readonly int MoveAngle = Animator.StringToHash("MoveAngle");
            public static readonly int StartAngle = Animator.StringToHash("StartAngle");
        
            public static readonly int MoveX = Animator.StringToHash("MoveX");
            public static readonly int MoveY = Animator.StringToHash("MoveY");
            public static readonly int IsRunning = Animator.StringToHash("IsRunning");
            public static readonly int IsMoving = Animator.StringToHash("IsMoving");
            public static readonly int IsTurnEnabled = Animator.StringToHash("IsTurnEnabled");
            public static readonly int HasRifle = Animator.StringToHash("HasRifle");

            public static readonly int IsAiming = Animator.StringToHash("IsAiming");
            
            public static readonly int IsLeftFootLowerRight = Animator.StringToHash("IsLeftFootLowerRight");
            
            
            public static readonly int WorldMoveX = Animator.StringToHash("WorldMoveX");
            public static readonly int WorldMoveY = Animator.StringToHash("WorldMoveY");
            
            public static readonly int AimAngle = Animator.StringToHash("AimAngle");

        }
        
       
    }
}