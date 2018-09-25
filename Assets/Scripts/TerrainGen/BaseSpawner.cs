using UnityEngine;

namespace TerrainGen
{
    public class BaseSpawner : MonoBehaviour
    {
        private MapPreview _mapPreview;
        public GameObject Jail;

        private void Awake()
        {
            _mapPreview = GetComponent<MapPreview>();
        }

        public void SpawnBase(Vector3 location)
        {
            Jail.transform.position = location;
        }
    }
}
