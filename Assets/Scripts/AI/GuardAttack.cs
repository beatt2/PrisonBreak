using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class GuardAttack : MonoBehaviour
    {
        private LegMoveAI _legMoveAI;


        private GameObject _leftArm;
        private GameObject _rightArm;

        private NavMeshAgent _navMeshAgent;

        private bool _onceLock;
        private bool _checkDistance;

        public float AttackDuration;
        public float RotationSpeed;
        private float _originalNavMeshSpeed;

        private bool _timerDone;
        private bool _stopRequest;

        private void Awake()
        {
            _legMoveAI = GetComponent<LegMoveAI>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
      
            _leftArm = _legMoveAI.FrontLegs[1];
            _rightArm = _legMoveAI.HindLegs[1];
            _originalNavMeshSpeed = _navMeshAgent.speed;
        }

        private void Update()
        {
            if (GameController.Instance.Player == null) return;
            transform.LookAt(GameController.Instance.GetPlayerLocation.position);
            if (!_checkDistance || _onceLock) return;
            if (!(_navMeshAgent.remainingDistance < 3)) return;
            StartCoroutine(StartSpin());
            _onceLock = true;

        }


        public void StateHasChanged(GuardManager.GuardState type)
        {
            if (type == GuardManager.GuardState.Triggerd)
            {
                _checkDistance = true;
            }
        }

        private IEnumerator StartSpin()
        {
            Quaternion temp1= _leftArm.transform.rotation;
            Quaternion temp2 = _rightArm.transform.rotation;

            StartCoroutine(AttackTimer());
 
            _navMeshAgent.speed = _originalNavMeshSpeed * 2;
            while (!_timerDone)
            {
                _leftArm.transform.Rotate(RotationSpeed,0,0, Space.Self);
                _rightArm.transform.Rotate(RotationSpeed, 0, 0, Space.Self);

                if (_stopRequest)
                {
                    _leftArm.transform.rotation = temp1;
                    _rightArm.transform.rotation = temp2;
                    _onceLock = false;
                    _checkDistance = false;
                    _stopRequest = false;
                    StartCoroutine(SlowDownTimer());
                    yield break;

                }
                yield return null;

            }
        }

        private IEnumerator AttackTimer()
        {
            yield return new WaitForSeconds(AttackDuration);
            _stopRequest = true;
        }

        private IEnumerator SlowDownTimer()
        {
            _navMeshAgent.speed = _originalNavMeshSpeed / 2;
            yield return  new WaitForSeconds(3);
            _navMeshAgent.speed = _originalNavMeshSpeed;

        }
    }
}

