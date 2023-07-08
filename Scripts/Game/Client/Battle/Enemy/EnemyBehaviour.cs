using UnityEngine;
using UnityEngine.AI;

namespace Game.Client.Battle
{
    public class EnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _target;
        [SerializeField] private float _health;

        private EnemyState _enemyState;
        private float _maxTime = 1f;
        private float _maxDistance = 2f;
        private float _timer;

        private string _moveAnim = "MoveSpeed";
        private string _hitAnim = "Hit";
        private string _attackAnim = "Attack";
        private string _diedAnim = "Died";

        private void Start()
        {
            _enemyState = EnemyState.Walk;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;

            switch (_enemyState)
            {
                case EnemyState.Walk:
                    MoveToTarget();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
                case EnemyState.Hit:
                    TakeDamage(1);
                    break;
            }
        }

        private void MoveToTarget()
        {
            if (_timer <= 0)
            {
                var sqrDistance = GetSqrDistance();
                if (sqrDistance > _maxDistance * _maxDistance)
                {
                    _agent.SetDestination(_target.position);
                }

                _timer = _maxTime;
            }

            if (Vector3.Distance(_target.position, transform.position) < _maxDistance)
            {
                _enemyState = EnemyState.Attack;
            }
            
            _animator.SetFloat(_moveAnim, _agent.velocity.magnitude);
        }

        private float GetSqrDistance()
        {
            return (_target.position - _agent.destination).sqrMagnitude;
        }

        private void Attack()
        {
            if (_timer <= 0)
            {
                _animator.SetTrigger(_attackAnim);
                _timer = _maxTime;
                
                if (GetSqrDistance() > _maxDistance * _maxDistance)
                {
                    _enemyState = EnemyState.Walk;
                }
            }
        }
        
        public void TakeDamage(int damage)
        {
            _health -= damage;
            _animator.SetTrigger(_hitAnim);

            if (_health <= 0)
            {
                _animator.applyRootMotion = true;
                _animator.SetTrigger(_diedAnim);
                _agent.enabled = false;
                DestroyEnemy();
            }
        }
        
        private void DestroyEnemy()
        {
            Destroy(gameObject, 1f);
        }
    }

    public enum EnemyState
    {
        Walk,
        Attack,
        Hit
    }
}