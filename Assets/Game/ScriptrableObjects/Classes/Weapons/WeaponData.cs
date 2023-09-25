using UnityEngine;
using static Common.Enums;

namespace Game.ScriptrableObjects.Classes.Weapons
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Data/Weapon/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] private WeaponType _type;
        [SerializeField] private TeamType   _team;
        [SerializeField] private BulletType _bulletType;
        [SerializeField] private Mesh       _mesh;
        [SerializeField] private int      _damage;
        [SerializeField] private int      _numberOfShots = 1;
        
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private float _shotDelay;

        public WeaponType Type => _type;
        public TeamType Team => _team;
        public BulletType BulletType => _bulletType;
        public Mesh Mesh => _mesh;
        public float BulletSpeed => _bulletSpeed;
        public int Damage => _damage;
        public int NumberOfShots => _numberOfShots;
        public float ShotDelay => _shotDelay;
    }
}