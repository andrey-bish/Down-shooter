using System.Collections;
using Data.Characters;
using UnityEngine;
using Utils;

namespace Characters.Enemies.Units
{
    
    public class EnemyBase : CharacterBase
    {
        private EnemyData Data => _enemyData;
        
        private EnemyData _enemyData;
        private Transform _playerTransform;

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
        
        
    }
}