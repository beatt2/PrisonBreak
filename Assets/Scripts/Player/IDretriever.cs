using Door;
using UnityEngine;

namespace Player
{
    public class IDretriever : MonoBehaviour
    {
        public Doors.AccessRuleEnum Type;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<IDoorBehaviour>() != null)
            {

                DoorManager.Instance.OpenDoor(other.gameObject.GetComponent<IDoorBehaviour>().ID(), Type);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<IDoorBehaviour>() != null)
            {
                DoorManager.Instance.CloseDoorTrigger(other.gameObject.GetComponent<Doors>(), Type);
            }
        }


    }
}
