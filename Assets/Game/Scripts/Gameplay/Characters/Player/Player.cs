using System;
using Characters.FireLogic;
using Damageable;
using Extensions;
using Providers;
using Unity.VisualScripting;
using UnityEngine;

namespace Characters.Player
{
    public class Player: CharacterBase
    {
        [GroupView] private FireBase _fireLogic;

        private int _weaponCount = 0;
        
        private void Awake()
        {
#if UNITY_ANDROID || UNITY_IOS
            _fireLogic = transform.AddComponent<MobileFire>();
#else
            _fireLogic = transform.AddComponent<PcFire>();
#endif
            _fireLogic.OnFire += Fire;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _fireLogic.OnFire -= Fire;
        }

        protected override void Start()
        {
            CameraProvider.SetPlayerTarget(transform);
            
            base.Start();
        }

        protected override void Fire(Vector3 shotEndPosition)
        {
            if (LevelFinished) return;
            CurrentWeapon.Fire(shotEndPosition);
        }

        protected override void AddTarget(IDamageable target)
        {
            
        }

        protected override void RemoveTarget(IDamageable target)
        {
            
        }

        public void UpgradeWeapon()
        {
            _weapons[_weaponCount].Deactivate();
            _weaponCount++;
            _weapons[_weaponCount].Activate();
            CurrentWeapon = _weapons[_weaponCount];
        }

        public void SelectWeapon(int value)
        {
            CurrentWeapon.Deactivate();
            _weapons[value].Activate();
            CurrentWeapon = _weapons[value];
        }

        public void StopGame() => LevelFinished = true;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                UpgradeWeapon();
            }
        }
        
        
    }
}