using UnityEngine;

namespace Door
{
    public class DoorLight : MonoBehaviour
    {
        public Material RedMaterial;
        public Material GreenMaterial;

        public int WaitForSeconds;

        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void Open()
        {
            _meshRenderer.material = GreenMaterial;
        }

        public void Close()
        {
            Invoke(nameof(CloseInvoke),WaitForSeconds);

        }

        private void CloseInvoke()
        {
            _meshRenderer.material = RedMaterial;
        }


    }
}