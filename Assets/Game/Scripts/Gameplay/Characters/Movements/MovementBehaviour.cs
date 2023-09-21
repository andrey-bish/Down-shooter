using System;
using UnityEngine;

namespace Characters.Movements
{
    public abstract class MovementBehaviour : MonoBehaviour
    {
        public abstract void Move(Vector3 input, Action OnEndPath = null);
        public abstract void Warp(Vector3 input);
        public bool IsMoving { get; protected set; }
        public virtual bool IsStopped => !IsMoving;
        public virtual Vector3 CurrentVelocity { get; }

        public virtual void Stop()
        {
            IsMoving = false;
        }

        public virtual void Resume()
        {
            IsMoving = true;
        }

        public virtual void Enable()
        {
            enabled = true;
        }

        public virtual void Disable()
        {
            enabled = false;
        }

        public virtual void SetSpeed(float waveSpeed)
        {
			
        }

        public virtual float GetSpeed()
        {
            return 0.0f;
        }
    }
}