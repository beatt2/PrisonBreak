using Player;
using UnityEngine.AI;

namespace AI
{
    public class LegMoveAI : LegMove
    {
        private NavMeshAgent _navMeshAgent;

        public override void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            base.Awake();
        }

        public override float GetAxis()
        {
            return _navMeshAgent.velocity.magnitude;
        }

    }
}
