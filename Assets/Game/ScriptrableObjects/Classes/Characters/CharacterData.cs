using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using static Common.Enums;

namespace Data.Characters
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Data/Characters/CharacterData")]
    public class CharacterData : ScriptableObject
    {
        [SerializeField] private TeamType      _team;

		[SerializeField, Group("Movement")] private float _walkSpeed = 2;
		[SerializeField, Group("Movement")] private float _runSpeed  = 6;

		[SerializeField, Group("Movement")]
		private float _angularSpeed = 1500;
		
		[SerializeField, Group("Movement")]
		private float _turnSmoothDuration = 0.1f;
		
		[SerializeField, Group("Movement")]
		private float _slowDownDuration = 0.2f;

		[SerializeField, Group("Movement")] private float _gravityForce     = 1;
		[SerializeField, Group("Movement")] private float _deadDuration     = 2;
		[SerializeField, Group("Movement")] private float _deadDownDuration = 1;
            
		[SerializeField, Group("Characteristics")] private float _maxHealth = 100;
		[SerializeField, Group("Characteristics")] private bool _isRegeneration;
		[SerializeField, Group("Characteristics"), ShowIf("_isRegeneration")] private float _regenerationHealth;
		[SerializeField, Group("Characteristics"), ShowIf("_isRegeneration")] private float _regenerationDelay;
		
		[SerializeField, Group("Interaction")]
		private float _actionInterval = 0.2f;

		[SerializeField, Group("Interaction")]
		private float _farZoneRadius = 4f;

		[SerializeField, Group("Interaction"), Min(0)]
		private float _stoppingDistance = 0f;

		public TeamType Team => _team;
		
		public float WalkSpeed => _walkSpeed;
		public float RunSpeed => _runSpeed;
		public float AngularSpeed => _angularSpeed;
		public float TurnSmoothDuration => _turnSmoothDuration;
		public float SlowDownDuration => _slowDownDuration;
		public float GravityForce => _gravityForce;
		public float DeadDuration => _deadDuration;
		public float DeadDownDuration => _deadDownDuration;
        
		public float MaxHealth => _maxHealth;
		public float RegenerationHealth => _regenerationHealth;
		public float RegenerationDelay => _regenerationDelay;
		public float ActionInterval => _actionInterval;
		public float FarZoneRadius => _farZoneRadius;
		public float StoppingDistance => _stoppingDistance;
    }
}