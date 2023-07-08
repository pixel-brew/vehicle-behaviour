using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Client.Battle
{
    public struct CharacterInput
    {
        public Vector2 Move;
        public Vector2 Aim;
        public bool IsAiming;
        public bool IsAttacking;
        public bool IsJumping;
        
        
    }
}