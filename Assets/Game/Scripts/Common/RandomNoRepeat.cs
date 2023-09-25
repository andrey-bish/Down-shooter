using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class RandomNoRepeat
    {
        private readonly List<int> _available = new();

        private int _count;
        private int _last;
        private int _iteration;
        private int _requestNum;

        private void Reset()
        {
            _available.Clear();
            _requestNum = 0;
            for (var i = 0; i < _count; i++) _available.Add(i);
            //Remove last, so it won't be repeated
            if (++_iteration > 1 && _count > 1)
            {
                _available.Remove(_last);
            }
            //Debug.Log("Reset");
        }

        public void Init(int value)
        {
            _iteration = 0;
            _count = value;
            Reset();
        }

        public int GetAvailable()
        {
            CheckAvailableIds();

            return GetAvailableAtId(Random.Range(0, _available.Count));
        }

        private int GetAvailableAtId(int availableId)
        {
            if (availableId < 0 || availableId >= _count) return -1;

            CheckAvailableIds();

            if (availableId >= _available.Count) Reset();

            var id = _available[availableId];
            _available.RemoveAt(availableId);

            //Adding back removed index after first request
            if (++_requestNum == 1 && _iteration > 1 && _count > 1) _available.Add(_last);
            _last = id;

            return id;
        }

        private void CheckAvailableIds()
        {
            if (_available.Count == 0) Reset();
        }
    }
}