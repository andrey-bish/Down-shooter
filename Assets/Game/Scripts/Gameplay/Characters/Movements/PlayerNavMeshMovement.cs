using System;
using Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Movements
{
    public class PlayerNavMeshMovement: MovementBehaviour
    {
        [SerializeField, GroupComponent] private NavMeshAgent _agent;
        [SerializeField, GroupComponent] private Animator  _animator;

        private readonly int _animIDForward = Animator.StringToHash("Forward");
        
        private Vector3 _input;
        
        private void FixedUpdate()
        {
            float distance = MoveInternal();
            RotateTowardsInput();
            SetAnimationSpeed(distance / (_agent.speed * Time.deltaTime));
        }

        public override void Move(Vector3 input, Action OnEndPath = null)
        {
            _input = input;
        }

        public override void Move(Transform moveTarget) => _input = moveTarget.position;

        public override void Warp(Vector3 input)
        {
            _agent.Warp(input);
        }

        private void RotateTowardsInput()
        {
            if (_input.sqrMagnitude < 0.01f) return;
            
            _agent.transform.rotation = Quaternion.LookRotation(_input, Vector3.up);
        }

        private float MoveInternal()
        {
            if (_input.sqrMagnitude < 0.01f)
            {
                IsMoving = false;
                return 0;
            }
            
            IsMoving = true;
            Transform agentTransform = _agent.transform;
            Vector3 position = agentTransform.position;
            Vector3 offset = _agent.speed * Time.deltaTime * _input.normalized;
            _agent.Move(offset);
            return (agentTransform.position - position).magnitude;
        }

        private void SetAnimationSpeed(float value)
        {
            _animator.SetFloat(_animIDForward, value);
        }
    }
}