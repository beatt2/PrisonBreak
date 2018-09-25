using UnityEngine;

namespace AI
{
    public class GuardColl : MonoBehaviour
    {
        private GuardAI _guardAI;

  
        private void Awake()
        {
            _guardAI = GetComponent<GuardAI>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            _guardAI.Health -= Mathf.Abs(other.rigidbody.velocity.magnitude / 2);
            other.rigidbody.velocity = Vector3.zero;
            GuardManager.Instance.TriggerGuards(GuardManager.LocationEnum.Butchery);
        }
    }
}