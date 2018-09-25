using UnityEngine;

namespace Items
{
    public class DynamiteVisual : MonoBehaviour
    {


        public GameObject MyDynamiteVisual;
        public GameObject RaycastTransform;
        private RaycastHit _hit;


        private void Start()
        {
            MyDynamiteVisual.SetActive(false);
            enabled = false;
        }

        public void Enable()
        {
            MyDynamiteVisual.SetActive(true);
            enabled = true;
        }

        public void Disable()
        {
            MyDynamiteVisual.SetActive(false);
            enabled = false;
        }

        private void FixedUpdate()
        {
            
            Ray myRay = new Ray(RaycastTransform.transform.position, RaycastTransform.transform.right);
            Debug.DrawLine(myRay.origin, myRay.GetPoint(5), Color.red);
            if (!Physics.Raycast(myRay, out _hit, 5)) return;
            if (!Input.GetKeyDown(KeyCode.Q)) return;
            if (_hit.collider.gameObject.CompareTag("Cow"))
            {
                InventoryManager.Instance.TransferDynamiteItem(_hit.collider.gameObject);
            }
        }
    }
}



