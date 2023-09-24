using System;
using Providers;
using UnityEngine;

namespace Characters.FireLogic
{
    public class FireBase: MonoBehaviour
    {
        public event Action<Vector3> OnFire;

        protected Camera Camera;

        private void Start()
        {
            Camera = CameraProvider.MainCamera;
        }
        
        protected void Fire(Vector3 pos) => OnFire?.Invoke(pos);
    }
}