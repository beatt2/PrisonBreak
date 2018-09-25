using UnityEngine;

namespace Items
{
    public class ItemPickUp : MonoBehaviour
    {
        public int Weight;
        public string Name;

    
        protected  virtual void Trigger(Item myItem)
        {
            InventoryManager.Instance.AddItem(myItem);
            Destroy(gameObject);
        }
    
    }
}
