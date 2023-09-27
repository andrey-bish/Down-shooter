using System.Collections;
using Common.ObjectPool;
using Damageable;
using Data.Characters;
using DG.Tweening;
using Extensions;
using Providers;
using UnityEngine;
using Utils;
using static Common.Enums;

namespace Characters.Enemies.Units
{
    
    public class EnemyBase : CharacterBase
    {
        private EnemyData Data => _enemyData;
        
        private EnemyData _enemyData;
        private Transform _playerTransform;
        private Coroutine _attackRoutine;
        
        private float _lastAttack;

        private bool _isStopGame;

        private float AttackDelay => Data.AttackDelay;

        public int PointsForDeath => Data.PointsForDeath;

        public void Init(EnemyData enemyData, Transform playerTransform)
        {
            _enemyData = enemyData;
            _playerTransform = playerTransform;
            _movement.Enable();
            _movement.Resume();
            InitHealth();
            _isStopGame = false;
        }
        
        public override void Restart()
        {
            base.Restart();
            transform.localScale = Vector3.one;
            StopAttackCor();
            LevelFinished = false;
        }

        private void Move(Transform moveTarget) => _movement.Move(moveTarget);

        private void Update()
        {
            if (IsDead || _isStopGame) return;
            Move(_playerTransform);
        }

        public void StopMovement()
        {
            _isStopGame = true;
            _movement.Disable();
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
            Pool.Get(PrefabProvider.GetParticlePrefab(ParticleType.EnemyDie), transform.position);
            transform.DOScale(0.0f, 0.25f);
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

                if (Time.time < _lastAttack + AttackDelay) continue;
                if (Vector3.Dot(transform.forward.XZOnly(),
                	    (_playerTransform.position - transform.position).XZOnly().normalized) < 0.8f)
                {
                	continue;
                }
                //TODO сделать через оружие
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