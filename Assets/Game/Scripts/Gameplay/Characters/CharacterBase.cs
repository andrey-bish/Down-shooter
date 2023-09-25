using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using Data.Characters;
using Characters.Movements;
using Common.ObjectPool;
using Damageable;
using Sirenix.OdinInspector;
using UnityEngine;
using Weapons;
using static Common.Enums;

namespace Characters
{
	[SelectionBase]
	public class CharacterBase : PoolItem, IDamageable 
	{
		public event Action<IDamageable> OnDie;

		[SerializeField] protected Transform _model;
		[SerializeField] protected MovementBehaviour _movement;
		[SerializeField] private Rigidbody _body;
		[SerializeField] protected CharacterAnimator _animator;
		[SerializeField] protected Health _health;
		[SerializeField] protected InteractionZone _farZone;
		[SerializeField] protected Transform _shotPosition;
		[SerializeField] protected List<Weapon> _weapons;

		[SerializeField, AssetList] private CharacterData _characterData;

		private Coroutine _attackRoutine;

		protected Weapon CurrentWeapon;
		protected readonly List<IDamageable> _attackList = new List<IDamageable>();

		public bool IsDead => _health != default && _health.IsEmpty;
		public float FarZoneRadius => _farZone.Radius;
		public Vector3 ModelPosition => _model.position;
		public Vector3 ShotTargetPosition => _shotPosition.position;
		public TeamType Team => Data.Team;
		public CharacterData Data => _characterData;
		public Rigidbody Body => _body;
		public CharacterAnimator Animator => _animator;
		protected bool LevelFinished { get; set; }

		protected virtual void Start()
		{
			InitZones();
			InitHealth();
			InitMovement();
			InitWeapon();
		}

		protected virtual void OnEnable() => Subscribe();
		protected virtual void OnDisable() => Unsubscribe();


		#region Init

		public virtual void Init(CharacterData data) => _characterData = data;

		private void InitZones() => _farZone.Init(Data.FarZoneRadius);

		protected void InitHealth() => _health.Init(Data.MaxHealth);

		private void InitWeapon()
		{
			if (_weapons.Count > 0) CurrentWeapon = _weapons[0];
		}

		private void InitMovement()
		{
			if (_movement != default) _movement.Enable();
		}

		protected virtual void Subscribe()
		{
			_health.OnEmpty += Die;
			_farZone.OnZoneEnter += OnFarZoneEnter;
			_farZone.OnZoneExit += OnFarZoneExit;
		}

		protected virtual void Unsubscribe()
		{
			_health.OnEmpty -= Die;
			_farZone.OnZoneEnter -= OnFarZoneEnter;
			_farZone.OnZoneExit -= OnFarZoneExit;
		}

		#endregion
		
		public void Move(Vector3 input) => _movement.Move(input);
		
		#region Attack

		private void OnFarZoneEnter(Collider other)
		{
			if (other.transform.TryGetComponent<IDamageable>(out var target)) AddTarget(target);
		}

		private void OnFarZoneExit(Collider other)
		{
			if (other.transform.TryGetComponent<IDamageable>(out var target)) RemoveTarget(target);
		}

		protected virtual void AddTarget(IDamageable target)
		{
			
		}

		protected virtual void RemoveTarget(IDamageable target)
		{
			
		}

		protected virtual void Fire(Vector3 shotEndPosition)
		{
			
		}

		#endregion

		public virtual void TakeDamage(float value)
		{
			if (IsDead) return;

			_health.Decrease(value);
		}

		public void TakeDamage(float value, Vector3 hitPosition)
		{
			if (IsDead) return;

			_health.Decrease(value);
			//if (!IsDead) _flasher.DoFlash();
		}

		public void SetMovementBehaviour(MovementBehaviour movement)
		{
			_movement.Disable();
			_movement = movement;
			_movement.Enable();
		}

		[Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
		protected virtual void Die()
		{
			if (_movement != default) _movement.Disable();
			_animator.DoDie(true);
			_body.isKinematic = true;
			_movement.Disable();
			OnDie?.Invoke(this);
			AfterDie();
		}

		protected virtual void AfterDie()
		{
		}



	}
}