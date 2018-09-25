using UnityEngine;

namespace Items
{
    public class DynamitePickUp : ItemPickUp
    {

        public float TimeTillExplosion;
        private DynamiteItem _myDynamiteItem;

        public void Start ()
        {
            _myDynamiteItem = new DynamiteItem(Name, Weight,TimeTillExplosion);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            if (InventoryManager.Instance.PlayerHasDynamite()) return;
            _myDynamiteItem.StartTimerIfValid(other);
            Trigger(_myDynamiteItem);

        }
	

    }
}
