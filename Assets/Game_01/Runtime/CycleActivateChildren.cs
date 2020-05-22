using System;
using System.Collections;
using UnityEngine;

namespace Game_01.Runtime
{
    public class CycleActivateChildren : MonoBehaviour
    {
        [SerializeField] private float _delay = 0.25f;
        private int _currActiveIdx;
        private Coroutine _routine;

        private void OnEnable()
        {
            TurnOffAll();
            _routine = StartCoroutine(CycleRoutine());
        }

        private void OnDisable()
        {
            Stop();
        }

        private void TurnOffAll()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        private IEnumerator CycleRoutine()
        {
            while (true)
            {
                var childCount = transform.childCount;
                if (_currActiveIdx < childCount)
                {
                    var oldActiveChild = transform.GetChild(_currActiveIdx);
                    oldActiveChild.gameObject.SetActive(false);
                }

                _currActiveIdx++;
                _currActiveIdx %= childCount;

                var newActiveChild = transform.GetChild(_currActiveIdx);
                newActiveChild.gameObject.SetActive(true);

                yield return new WaitForSeconds(_delay);
            }
        }

        public void Stop()
        {
            if (_routine != null) StopCoroutine(_routine);
            _routine = null;
        }
    }
}