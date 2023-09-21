using System;
using Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Characters
{
    public class CharacterAnimator : MonoBehaviour
    {
        public event Action OnHit;
		public event Action OnPush;
		public event Action OnFallingComplete;
		public event Action OnGettingUpComplete;
		
		[SerializeField, GroupComponent] private Animator _animator;

		private static readonly int dead        = Animator.StringToHash("Dead");
		private static readonly int attack      = Animator.StringToHash("Attack");
		private static readonly int target      = Animator.StringToHash("Target");
		private static readonly int horizontal  = Animator.StringToHash("Horizontal");
		private static readonly int vertical    = Animator.StringToHash("Vertical");
		private static readonly int idle        = Animator.StringToHash("Idle");
		private static readonly int boss        = Animator.StringToHash("Boss");
		private static readonly int idleIndex   = Animator.StringToHash("IdleIndex");
		private static readonly int idleCount   = Animator.StringToHash("IdleCount");
		private static readonly int attackSpeed = Animator.StringToHash("AttackSpeed");
		private static readonly int speed       = Animator.StringToHash("Speed");

		public void DoDie(bool value) => _animator.SetBool(dead, value);
		public void DoAttack(bool value) => _animator.SetBool(attack, value);
		public void DoTarget(bool value) => _animator.SetBool(target, value);
		public void DoBoss(bool value) => _animator.SetBool(boss, value);

		public void DoIdle(bool value)
		{
			var indexCount = _animator.GetInteger(idleCount);
			if (value) _animator.SetInteger(idleIndex, Random.Range(0, indexCount));
			_animator.SetBool(idle, value);
		}

		public void SetAttackSpeed(float value) => _animator.SetFloat(attackSpeed, value);
		public void SetSpeed(float value) => _animator.SetFloat(speed, value);

		private void SetTrigger(int id)
		{
			if (_animator.GetCurrentAnimatorStateInfo(0).shortNameHash != id) _animator.SetTrigger(id);
		}

		private void FallingComplete() => OnFallingComplete?.Invoke();

		private void GettingUpComplete() => OnGettingUpComplete?.Invoke();

		public void SetDirection(Vector2 dir)
		{
			_animator.SetFloat(horizontal, dir.x);
			_animator.SetFloat(vertical, dir.y);
		}

		public void Hit() => OnHit?.Invoke();
		public void Push() => OnPush?.Invoke();
    }
}