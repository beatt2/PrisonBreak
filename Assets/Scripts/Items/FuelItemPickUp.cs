using UnityEngine;

namespace Items
{
    public class FuelItemPickUp : ItemPickUp
    {
        private FuelItem _myFuelItem;

        public float FuelValue;

        public void Start()
        {
            _myFuelItem = new FuelItem(Name, Weight, FuelValue);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            Trigger(_myFuelItem);

        }

        protected override void Trigger(Item myItem)
        {
            if (!InventoryManager.Instance.AddItem(myItem)) return;
            if (InventoryManager.Instance.GetFuelValue() + FuelValue < 1)
            {
                InventoryManager.Instance.AddFuel(FuelValue);
                Destroy(gameObject);

            }
            else if (InventoryManager.Instance.GetFuelValue() < 1)
            {
                FuelValue = 1;
                InventoryManager.Instance.AddFuel(FuelValue);
                Destroy(gameObject);
            }
        }
    }
}
