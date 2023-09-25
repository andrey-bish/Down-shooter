using System.Collections;
using Damageable;
using Data.Characters;
using Extensions;
using UnityEngine;
using Utils;

namespace Characters.Enemies.Units
{
    
    public class EnemyBase : CharacterBase
    {
        private EnemyData Data => _enemyData;
        
        private EnemyData _enemyData;
        private Transform _playerTransform;
        private Coroutine _attackRoutine;
        
        private float _lastAttack;

        private float AttackDelay => Data.AttackDelay;

        public void Init(EnemyData enemyData, Transform playerTransform)
        {
            _enemyData = enemyData;
            _playerTransform = playerTransform;
            _movement.Enable();
            _movement.Resume();
            InitHealth();
            //Move(_playerTransform);
        }
        
        public override void Restart()
        {
            base.Restart();
            StopAttackCor();
        }

        private void Move(Transform moveTarget)
        {
            _movement.Move(moveTarget);
        }

        private void Update()
        {
            if (_health.IsEmpty) return;
            Move(_playerTransform);
        }

        public override void TakeDamage(float value)
        {
            if (IsDead) return;

            _health.Decrease(value);
        }

        #region Die

        protected override void Die()
        {
            base.Die();
            StopAttackCor();
            AfterDie();
        }
        
        protected override void AfterDie()
        {
            StartCoroutine(DieCor());
        }
        
        private IEnumerator DieCor()
        {
            yield return Helper.GetWait(Data.DeadDuration);
            Release();
        }

        #endregion

        #region Attack

        protected override void AddTarget(IDamageable target)
        {
            if (LevelFinished) return;
            if (IsDead || target.IsDead) return;
            if (target.Team == Data.Team) return;
            
            target.OnDie += RemoveTarget;
            _attackRoutine ??= StartCoroutine(AttackCor(target));
            _animator.DoAttack(true);
            _movement.Stop();
        }

        protected override void RemoveTarget(IDamageable target)
        {
            if (target.Team == Data.Team) return;

            target.OnDie -= RemoveTarget;
            
            if (IsDead) return;
            _animator.DoAttack(false);
            StopAttackCor();
            if (LevelFinished) return;
            _movement.Resume();
        }
        
        private IEnumerator AttackCor(IDamageable target)
        {

            while (true)
            {
                yield return null;

                if (LevelFinished) yield break;

                // var position = transform.position;
                // _target = _attackList[0];
                // // var hasTarget = nearestTarget != default;
                // // _lastHasTarget = hasTarget;
                //
                // var characterPosition = _target.ModelPosition;
                //             
                if (Time.time < _lastAttack + AttackDelay) continue;
                if (Vector3.Dot(transform.forward.XZOnly(),
                	    (_playerTransform.position - transform.position).XZOnly().normalized) < 0.8f)
                {
                	continue;
                }
                Attack(target);
            }
        }

        private void StopAttackCor()
        {
            _attackRoutine.Stop(this);
            _attackRoutine = null;
        }
        
        private void Attack(IDamageable target)
        {
            target.TakeDamage(Data.Damage);
            _lastAttack = Time.time;
        }

        #endregion
        
    }
}