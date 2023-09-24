﻿using System;
using Characters.FireLogic;
using Extensions;
using Providers;
using Unity.VisualScripting;
using UnityEngine;

namespace Characters.Player
{
    public class Player: CharacterBase
    {
        [GroupView] private FireBase _fireLogic;

        private void Awake()
        {
#if UNITY_ANDROID || UNITY_IOS
            //добавить скрипт стрельбы на мобилку
            _fireLogic = transform.AddComponent<MobileFire>();
#else
            //добавить скрипт стрельбы на пк
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
            CurrentWeapon.Fire(shotEndPosition);
        }
    }
}