using UnityEngine;

namespace TerrainGen
{
    public class ObjectSize : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            Debug.Log(_meshRenderer.bounds.size);
        }
    }
}
