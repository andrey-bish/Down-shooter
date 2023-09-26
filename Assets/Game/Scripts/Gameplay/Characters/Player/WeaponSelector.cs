using System;
using UnityEngine;

namespace Characters.Player
{
    public class WeaponSelector
    {
        public event Action<int> OnSelectWeapon;
        
        public void CheckSelectWeapon()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                OnSelectWeapon?.Invoke(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                OnSelectWeapon?.Invoke(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                OnSelectWeapon?.Invoke(2);
            }
        }
    }
}