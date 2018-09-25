using Player;
using UnityEngine;

namespace Cow
{
    public class Head : MonoBehaviour
    {
        private LegMove _legMove;
        private Animator _animator;

        private bool _moveLeg;
        private float _waitForSeconds;

        private float GetSpeed => _legMove != null ? _legMove.GetAxis() : 0;

        private void Awake()
        {
            _legMove = transform.parent.parent.GetComponent<LegMove>();
            _animator = GetComponent<Animator>();
            _waitForSeconds = Random.Range(0, 10);
            Invoke(nameof(WaitForSeconds), _waitForSeconds);
        }

        private void Update()
        {
            if (_moveLeg)
                _animator.SetFloat("Speed", GetSpeed);
        }

        private void WaitForSeconds()
        {
            _moveLeg = true;
        }

    }
}
