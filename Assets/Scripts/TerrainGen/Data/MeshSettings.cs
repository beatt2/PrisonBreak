using UnityEngine;

namespace TerrainGen.Data
{

    [CreateAssetMenu]
    public class MeshSettings : UpdateableData
    {
        public const int NumSupportedLODs = 5;
        private const int NumSupportedChunkSizes = 9;
        private const int NumSupportedFlatshadedChunkSizes = 3;
        private static readonly int[] SupportedChunkSizes = { 48, 72, 96, 120, 144, 168, 192, 216, 240 };


        [Range(0, NumSupportedChunkSizes - 1)]
        public int ChunkSizeIndex;

        [Range(0, NumSupportedFlatshadedChunkSizes - 1)]
        public int FlatshadedChunkSizeIndex;

        public float MeshScale = 2.5f;
        public bool UseFlatShading;

        public int NumVertsPerLine => SupportedChunkSizes[(UseFlatShading) ? FlatshadedChunkSizeIndex : ChunkSizeIndex] + 1;
        public float MeshWorldSize => (NumVertsPerLine - 3) * MeshScale;
    }
}
