using UnityEngine;

namespace AI
{
    public class GuardTrigger : MonoBehaviour   
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            GuardManager.Instance.TriggerGuards(GuardManager.LocationEnum.Gatehouse);
            CowManager.Instance.SendAllToPosition(CowManager.CowLocationEnum.Field,LocationAI.Instance.GetPositionCow(2));
        }
    }
}
