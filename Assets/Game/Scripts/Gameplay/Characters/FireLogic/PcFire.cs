using System;
using UnityEngine;

namespace Characters.FireLogic
{
    public class PcFire: FireBase
    {
        private void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Ray mouseRay = Camera.ScreenPointToRay(Input.mousePosition);
                Plane plane = new Plane( Vector3.up, transform.position + Vector3.up * 1.5f);
                if( plane.Raycast( mouseRay, out float hitDist) ){
                    Vector3 hitPoint = mouseRay.GetPoint( hitDist );
                    Fire(hitPoint);
                }
            }
        }
    }
}