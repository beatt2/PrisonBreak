using UnityEngine;

namespace Items
{
    public class AccessItemPickUp : ItemPickUp
    {
        // ReSharper disable once InconsistentNaming
        public int DoorID;

        private AccessItem _accessItem;

        public void Start()
        {
            _accessItem = new AccessItem(Name, Weight, DoorID);
        }

        public void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
            Trigger(_accessItem);
        }

    
    }
}
