using System.Diagnostics.CodeAnalysis;
using Tools;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class _AI : MonoBehaviour
    {
        protected NavMeshAgent NavMeshAgent;

        private bool _update;
        private Transform _updateTransform;


        private bool _partol;



        protected virtual void Awake()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        protected virtual void Update()
        {
            if (_update)
            {
                NavMeshAgent.SetDestination(_updateTransform.position);
            }

            if (_partol)
            {
                PatrolFence();
            }
        }

        public void Stop()
        {
            NavMeshAgent.isStopped = true;
        }


        public void SendToLocation(Vector3 position)
        {
            NavMeshAgent.destination = position;
        }

        public void SendToLocation(Transform position, bool update)
        {
            _updateTransform = position;
            _update = update;
        }

        public void ChangeNavMesh(float speed, float accelaration, float angularSpeed, float stoppingDistance, bool autoBrake)
        {
            NavMeshSpeed = speed;
            Acceleration = accelaration;
            AngularSpeed = angularSpeed;
            StoppingDistance = stoppingDistance;
            AutoBrake = autoBrake;
        }

        public void SendToClosesPosition(Vector3 position, Transform[] targets)
        {

            NavMeshAgent.SetDestination(ClosestPoint.GetClosestPositionVector3(position, targets));
        }


        private void PatrolFence()
        {
            int _currentIndex = 0;

            if (!(NavMeshAgent.remainingDistance <= StoppingDistance)) return;
            _currentIndex++;
            if (_currentIndex == LocationAI.Instance.FencePath.Length)
            {
                _currentIndex = 0;
            }
            NavMeshAgent.SetDestination(LocationAI.Instance.FencePath[_currentIndex].position);
        }


        public float NavMeshSpeed
        {
            get { return NavMeshAgent.speed; }
            set { NavMeshAgent.speed = value; }
        }

        public float Acceleration
        {
            get { return NavMeshAgent.acceleration; }
            set { NavMeshAgent.acceleration = value; }
        }

        public float AngularSpeed
        {
            get { return NavMeshAgent.angularSpeed; }
            set { NavMeshAgent.angularSpeed = value; }
        }

        public float StoppingDistance
        {
            get { return NavMeshAgent.stoppingDistance; }
            set { NavMeshAgent.stoppingDistance = value; }
        }

        public bool AutoBrake
        {
            get { return NavMeshAgent.autoBraking; }
            set { NavMeshAgent.autoBraking = value; }
        }


    }
}
