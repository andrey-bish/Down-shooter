using System;
using DG.Tweening;
using Extensions;
using UnityEngine;

namespace Characters.Movements
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class PhysicsMovement: MovementBehaviour
    {
        [SerializeField, GroupComponent] private CharacterBase _character;
		[SerializeField, GroupSetting]   private bool          _physicVelocityAnimation;

		[GroupView] private float Speed => _character.Data.RunSpeed;
		private float TurnSmoothTime => _character.Data.TurnSmoothDuration;
		private float SlowDownTime => _character.Data.SlowDownDuration;
		private float GravityForce => _character.Data.GravityForce;

		private float     _turnSmoothVelocity;
		private Transform _cachedTransform;
		private Vector3   _moveVector;
		private Tween     _slowDownTween;
		private Vector3   _input;

		private readonly int _speed = Animator.StringToHash("Speed");

		private Vector3 PhysicsVelocity => _character.Body.velocity;
		public override Vector3 CurrentVelocity => _physicVelocityAnimation ? PhysicsVelocity : _input * Speed;

		public override bool IsStopped => _input == Vector3.zero;

		private void Awake() => _cachedTransform = transform;
		public override void Move(Vector3 input, Action OnEndPath = null) => _input = input;
		public override void Warp(Vector3 position) => _character.Body.position = position;

		public override void Disable()
		{
			base.Disable();
			_input = Vector3.zero;
			_character.Body.velocity = Vector3.zero;
			_character.Animator.SetSpeed(0);
		}

		private void FixedUpdate()
		{
			CheckForMove();
			CheckForRotate();
			Animate();
		}

		private void CheckForMove()
		{
			if (_input.sqrMagnitude > 0.01f)
				MoveTowards(_input);
			else
				SlowDown();
		}

		private void SlowDown()
		{
			var slowMagnitude = Mathf.Min(Speed / SlowDownTime * Time.deltaTime, PhysicsVelocity.magnitude);

			_character.Body.AddForce(Vector3.down * GravityForce - PhysicsVelocity.normalized * slowMagnitude,
															 ForceMode.VelocityChange);
		}

		private void MoveTowards(Vector3 input)
		{
			var horizontalVelocity = _character.Body.velocity.XZOnly();

			_character.Body.AddForce(Vector3.down * GravityForce + input * Speed - horizontalVelocity,
															 ForceMode.VelocityChange);
		}

		private void CheckForRotate()
		{
			if (CurrentVelocity.XZOnly().sqrMagnitude > 0.01f) Rotate();
		}

		private void Rotate()
		{
			var targetAngle = Vector3.SignedAngle(Vector3.forward, CurrentVelocity, Vector3.up);
			var angle = Mathf.SmoothDampAngle(_cachedTransform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
																				TurnSmoothTime);
			_cachedTransform.rotation = Quaternion.Euler(0, angle, 0);
		}

		private void Animate()
		{
			_character.Animator.SetSpeed(CurrentVelocity.magnitude);
		}
    }
}