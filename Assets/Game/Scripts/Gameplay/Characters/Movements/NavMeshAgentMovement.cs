using System;
using System.Collections;
using Extensions;
using Characters.Movements;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Movements
{
    public class NavMeshAgentMovement: MovementBehaviour
    {
        [SerializeField] private CharacterAnimator _animator;
		[SerializeField] private NavMeshAgent      _agent;
		[SerializeField] private Rigidbody         _rigidbody;

		private float Velocity => _agent == default ? 0 : _agent.velocity.magnitude;
		private float DesiredVelocity => _agent == default ? 0 : _agent.desiredVelocity.magnitude;
		private Coroutine _moveCor;
		private WaitForSeconds _wait;
		private bool FarAway => Vector3.Distance(transform.position.XZOnly(), _agent.pathEndPosition) > 4.4f;
		public override bool IsStopped => _agent.velocity.sqrMagnitude == 0;
		public override Vector3 CurrentVelocity => _agent.velocity;

		private float _speed;

		private void Start()
		{
			_wait = new WaitForSeconds(0.5f);
		}

		public override void Move(Vector3 input, Action OnEndPath = null)
		{
			if (_agent.enabled && _agent.isOnNavMesh)
			{
				_agent.SetDestination(input);
				_moveCor = StartCoroutine(MoveToPoint(OnEndPath));
			}
		}

		public override void Move(Transform moveTarget)
		{
			if (_agent.enabled && _agent.isOnNavMesh)
			{
				_agent.SetDestination(moveTarget.position);
			}
		}

		private IEnumerator MoveToPoint(Action OnEndPath)
		{
			while (true)
			{
				yield return _wait;
				if (_agent.IsReached())
				{
					OnEndPath?.Invoke();
				}
			}
		}

		private float GetDistance(Vector3 to)
		{
			return Vector3.Distance(transform.position.XZOnly(), to);
		}
		
		private void Update()
		{
			if (_agent.isStopped)
				_animator.SetSpeed(_rigidbody.velocity.XZOnly().magnitude);
			else
				_animator.SetSpeed(_agent.desiredVelocity.magnitude);
		}

		public override void Warp(Vector3 input)
		{
			if (_agent.enabled && _agent.isOnNavMesh) _agent.Warp(input);
		}

		public override void Stop()
		{
			base.Stop();
			//_agent.speed = 0;
			if (_agent.enabled) _agent.isStopped = true;
		}

		public override void Resume()
		{
			base.Resume();
			//_agent.speed = _speed;
			if (_agent.enabled && _agent.isOnNavMesh) _agent.isStopped = false;
		}

		public override void Enable()
		{
			base.Enable();
			_agent.enabled = true;
		}

		public override void Disable()
		{
			if (_agent.enabled && _agent.isOnNavMesh) _agent.ResetPath();
			_agent.enabled = false;
			_animator.SetSpeed(0);
			base.Disable();
		}

		public override void SetSpeed(float waveSpeed)
		{
			_agent.speed = waveSpeed;
			_speed = waveSpeed;
		}

		public override float GetSpeed() => _agent.speed;
    }
}