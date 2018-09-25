using System.Collections;
using AI;
using Player;
using UnityEngine;

namespace Spike
{
    public class SpikesTrigger : MonoBehaviour
    {
        public int SecondsToWaitFor;

        private bool _moving;
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Cow") && !other.gameObject.CompareTag("Player")) return;
            if(!_moving)
                StartCoroutine(WaitIE());
         
            _moving = true;
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerController>().Health -= 100;
            }
        }

        private IEnumerator WaitIE()
        {
            yield return new WaitForSeconds(SecondsToWaitFor);
            EffectManager.Instance.InstansiateExplosion(transform);
            CowManager.Instance.FinishedButchering();
            _moving = false;

        }
    }
}
