using Extensions;
using UnityEngine;

namespace Data.Characters
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Data/Characters/EnemyData")]
    public class EnemyData: CharacterData
    {
        
        [SerializeField, Group("Attack")] private float _attackDelay;
        [SerializeField, Group("Attack")] private int _damage;
        
        [SerializeField, Group("Points")] private int _pointsForDeath;

        public float AttackDelay => _attackDelay;
        public int Damage => _damage;
        public int PointsForDeath => _pointsForDeath;
    }
}