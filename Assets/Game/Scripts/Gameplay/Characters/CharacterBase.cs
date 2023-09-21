using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using Data.Characters;
using Characters.Movements;
using Damageable;
using Sirenix.OdinInspector;
using UnityEngine;

using static Common.Enums;

namespace Characters
{
	public class CharacterBase : MonoBehaviour, IDamageable

	{
		public event Action<IDamageable> OnDie;

		[SerializeField] protected Transform _model;
		[SerializeField] protected MovementBehaviour _movement;
		[SerializeField] private Rigidbody _body;
		[SerializeField] protected CharacterAnimator _animator;
		[SerializeField] protected Health _health;
		[SerializeField] protected InteractionZone _farZone;
		[SerializeField] protected Transform _shotPosition;

		[SerializeField, AssetList] private CharacterData _characterData;

		private Coroutine _attackRoutine;

		//protected Weapon CurrentWeapon;
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
		}

		protected virtual void OnEnable() => Subscribe();
		protected virtual void OnDisable() => Unsubscribe();


		#region Init

		public virtual void Init(CharacterData data) => _characterData = data;

		private void InitZones() => _farZone.Init(Data.FarZoneRadius);

		private void InitHealth() => _health.Init(Data.MaxHealth);

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
			if (LevelFinished) return;
			if (IsDead || target.IsDead) return;
			if (target.Team == Data.Team) return;

			_attackList.Add(target);
			target.OnDie += RemoveTarget;
			_animator.DoAttack(true);
			_movement.Stop();
			_attackRoutine ??= StartCoroutine(AttackCor());
		}

		protected virtual void RemoveTarget(IDamageable target)
		{
			if (target.Team == Data.Team) return;

			target.OnDie -= RemoveTarget;
			_attackList.Remove(target);
			if (_attackList.Count != 0) return;
			if (IsDead) return;
			_animator.DoAttack(false);

			StopAttackCor();
			if (LevelFinished) return;
			_movement.Resume();
		}

		private IEnumerator AttackCor()
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
				// if (Time.time < _lastAttack + ShotDelay) continue;
				// if (Vector3.Dot(transform.forward.XZOnly(),
				// 	    (characterPosition - transform.position).XZOnly().normalized) < 0.8f)
				// {
				// 	continue;
				// }
			}
		}

		private void StopAttackCor()
		{
			_attackRoutine.Stop(this);
			_attackRoutine = null;
		}

		#endregion

		public void TakeDamage(float value)
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