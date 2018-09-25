using UnityEngine;

namespace TerrainGen.Data
{
    [CreateAssetMenu]
    public class ObjectSpawnData : UpdateableData
    {
        public int AmountToSpawn;
        public int MaxDistance;
        public GameObject ObjectToSpawn;
        public bool Collider;
        public bool UseTrigger;
        public float StartHeight;
        public float EndHeight;
    }
}
