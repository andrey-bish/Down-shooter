using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public static class Helper
    {
        #region WaitDictionary
        
        private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
        
        public static WaitForSeconds GetWait(float time)
        {
            if (WaitDictionary.TryGetValue(time, out var wait))
                return wait;

            WaitDictionary[time] = new WaitForSeconds(time);
            return WaitDictionary[time];
        }
        
        public static IEnumerator WaitCoroutine(float duration, Action finished = null)
        {
            yield return GetWait(duration);
            finished?.Invoke();
        }
        
        #endregion

        #region ShakeRotation

        public static void ShakeRotation(this Sequence sequence, Transform shakeTransform, float strong = 24.0f)
        {
            sequence
                .Append(shakeTransform.DOLocalRotate(Vector3.zero, 0.1f))
                .Append(shakeTransform.DOLocalRotate(new(0.0f, 0.0f, strong + 2), 0.075f))
                .Append(shakeTransform.DOLocalRotate(new(0.0f, 0.0f, -strong), 0.075f))
                .Append(shakeTransform.DOLocalRotate(new(0.0f, 0.0f, strong), 0.075f))
                .Append(shakeTransform.DOLocalRotate(new(0.0f, 0.0f, strong + 2), 0.075f))
                .Append(shakeTransform.DOLocalRotate(new(0.0f, 0.0f, strong), 0.075f))
                .Append(shakeTransform.DOLocalRotate(Vector3.zero, 0.1f));
        }

        #endregion
    }
}