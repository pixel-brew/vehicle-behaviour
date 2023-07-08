using System;
using Core;
using UnityEngine;

namespace Game.Client.Battle
{
    public class StartAngleSetter : StateMachineBehaviour
    {

        private int _timeCounter;

        private Vector2 _moveVector;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            UpdateStartAngle(animator, 0);
            // Debug.Log("OnStateEnter " + stateInfo.normalizedTime);
            
        }

        private void UpdateStartAngle(Animator animator, float normalizedTime)
        {
            var angle = animator.GetFloat(CharacterAnimator.Variable.MoveAngle);
            animator.SetFloat(CharacterAnimator.Variable.StartAngle, angle);
            _moveVector.x = animator.GetFloat(CharacterAnimator.Variable.WorldMoveX);
            _moveVector.y = animator.GetFloat(CharacterAnimator.Variable.WorldMoveY);
            
            _timeCounter = (int)normalizedTime + 1;
        }


        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var curMoveVector = new Vector2(animator.GetFloat(CharacterAnimator.Variable.WorldMoveX),animator.GetFloat(CharacterAnimator.Variable.WorldMoveY));
            
            
            // DebugDrawer.DrawLine3d(animator.transform.position, animator.transform.position + new Vector3(curMoveVector.x,0f,curMoveVector.y) * 5f, Color.red, Time.deltaTime);
            // DebugDrawer.DrawLine3d(animator.transform.position, animator.transform.position + new Vector3(_moveVector.x,0f,_moveVector.y) * 5f, Color.green, Time.deltaTime);
            var angleDif = Math.Abs(Angle.Normalize(Vector2.SignedAngle(_moveVector, curMoveVector)));
            
            
            if (angleDif > 5)
            {
                UpdateStartAngle(animator, stateInfo.normalizedTime);
            }
           
            if (stateInfo.normalizedTime > _timeCounter)
            {
                UpdateStartAngle(animator, stateInfo.normalizedTime);
            }
            
            // DebugDrawer.DrawText3d(animator.transform.position + Vector3.up*10f, Color.green, $"moveAngle: {animator.GetFloat(CharacterAnimator.Variable.MoveAngle)}\n startAngle : {animator.GetFloat(CharacterAnimator.Variable.StartAngle)} \n " +
            //                                                                                $"moveVector: {_moveVector}", Time.deltaTime);
            
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}