using System;
using Damageable;
using UnityEngine;
using static Common.Enums;

namespace Gameplay.Enviromentals
{
    public class Buildings:MonoBehaviour, IDamageable
    {
        public event Action<IDamageable> OnDie;
        public TeamType Team => TeamType.Neutral;
        public bool IsDead { get; }
        public Vector3 ModelPosition { get; }
        public Vector3 ShotTargetPosition { get; }
        
        public void TakeDamage(float value)
        {
        }

        public void TakeDamage(float value, Vector3 hitPosition = default)
        {
        }
    }
}