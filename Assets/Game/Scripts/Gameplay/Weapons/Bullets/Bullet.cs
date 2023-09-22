﻿using Common.ObjectPool;
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
        [SerializeField, GroupComponent] protected Gradient _gradient;
        [SerializeField, GroupComponent] protected Gradient _gradientBoost;
        [SerializeField, GroupComponent] protected Gradient _gradientSmoke;
        [SerializeField, GroupComponent] protected Gradient _gradientBoostSmoke;
        [SerializeField, GroupSetting] protected ParticleType _trailType = ParticleType.Trail;
        [SerializeField, GroupSetting] protected float _trailReleaseDelay;

        [GroupView] protected TeamType       _team;
        [GroupView] protected Vector3        _direction;
        [GroupView] protected float          _speed;
        [GroupView] protected int          _damage;
        [GroupView] protected PooledParticle _trail;
        [GroupView] private bool _isHit;
        [GroupView] protected bool _isBoost;

        public Rigidbody Rigidbody
        {
            get => _rigidbody;
            protected set => _rigidbody = value;
        }

        public virtual void Init(Vector3 direction, float speed, int damage, TeamType team, bool isBoost)
        {
            _direction = direction;
            _speed = speed;
            _damage = damage;
            _team = team;
            _isBoost = isBoost;
            transform.LookAt(transform.position + direction);
            if(_trail) _trail.SetTrailColor(_isBoost ? _gradientBoost : _gradient, _isBoost ? _gradientBoostSmoke : _gradientSmoke);
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
            _trail.SetParent(Pool.GetContainer(_trail.ID));
            _trail.ReleaseAfter(_trailReleaseDelay);
            base.Release(disableObject);
        }

        protected virtual void FixedUpdate()
        {
            _rigidbody.MovePosition(_rigidbody.position + _direction.normalized * _speed);
        }

        private void SetTrail()
        {
            var prefab = PrefabProvider.GetParticlePrefab(_trailType);
            _trail = Pool.Get(prefab, transform.position, transform);
            _trail.SetTrailColor(_isBoost ? _gradientBoost : _gradient, _isBoost ? _gradientBoostSmoke : _gradientSmoke);
            _trail.StopRelease();
        }
        
        protected virtual void OnTriggerEnter(Collider collider)
        {
            if (collider.TryGetComponent(out IDamageable target))
            {
                if(_isHit) return;
                if (target.Team == _team) return;
                target.TakeDamage(_damage, transform.position);
                _isHit = true;
                Release();
            }
        }
    }
}