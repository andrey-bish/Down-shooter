using Common.ObjectPool;
using Damageable;
using Extensions;
using Providers;
using UnityEngine;
using static Common.Enums;

namespace Weapons.Bullets
{
    public class Bullet : PoolItem
    {
        [SerializeField, GroupComponent] private Rigidbody _rigidbody;
        [SerializeField, GroupSetting] protected ParticleType _trailType = ParticleType.Trail;
        [SerializeField, GroupSetting] protected float _trailReleaseDelay;

        [GroupView] protected TeamType       _team;
        [GroupView] protected Vector3        _direction;
        [GroupView] protected float          _speed;
        [GroupView] protected int          _damage;
        [GroupView] protected PooledParticle _trail;
        [GroupView] private bool _isHit;

        public Rigidbody Rigidbody
        {
            get => _rigidbody;
            protected set => _rigidbody = value;
        }

        public virtual void Init(Vector3 direction, float speed, int damage, TeamType team)
        {
            _direction = direction;
            _speed = speed;
            _damage = damage;
            _team = team;
            transform.LookAt(transform.position + _direction);
        }

        public override void Retain(int id, string containerName)
        {
            base.Retain(id, containerName);
            SetTrail();
        }

        public override void Restart()
        {
            _isHit = false;
            base.Restart();
            SetTrail();
        }

        public override void Release(bool disableObject = true)
        {
            // _trail.SetParent(Pool.GetContainer(_trail.ID));
            // _trail.ReleaseAfter(_trailReleaseDelay);
            base.Release(disableObject);
        }

        protected virtual void FixedUpdate()
        {
            _rigidbody.MovePosition(transform.position + _direction * _speed * Time.deltaTime);
        }

        private void SetTrail()
        {
            // var prefab = PrefabProvider.GetParticlePrefab(_trailType);
            // _trail = Pool.Get(prefab, transform.position, transform);
            // _trail.StopRelease();
        }
        
        protected virtual void OnTriggerEnter(Collider collider)
        {
            if (collider.TryGetComponent(out IDamageable target))
            {
                if(_isHit) return;
                if (target.Team == _team) return;
                target.TakeDamage(_damage);
                _isHit = true;
                Release();
            }
        }
    }
}