using System.Collections;
using UnityEngine;

namespace Common
{
    public class Coroutines : MonoBehaviour
    {
        private static Coroutines _instance
        {
            get
            {
                if (m_instance == null)
                {
                    var go = new GameObject("[COROUTINE MANAGER]");
                    m_instance = go.AddComponent<Coroutines>();
                    DontDestroyOnLoad(go);
                }
                return m_instance;
            }
        }

        private static Coroutines m_instance;

        public static Coroutine StartRoutine(IEnumerator enumerator)
        {
            return _instance.StartCoroutine(enumerator);
        }

        public static void StopRoutine(Coroutine routine)
        {
            if(routine != null)
                _instance.StopCoroutine(routine);
        }
    }
}