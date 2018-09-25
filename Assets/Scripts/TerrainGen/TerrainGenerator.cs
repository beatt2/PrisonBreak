using System.Collections.Generic;
using TerrainGen.Data;
using UnityEngine;

namespace TerrainGen
{
    public class TerrainGenerator : MonoBehaviour
    {


        private const float ViewerMoveTresholdForChunkUpdate = 25f;
        private const float SqrViewerMoveTresholdForChunkUpdate = ViewerMoveTresholdForChunkUpdate * ViewerMoveTresholdForChunkUpdate;


        public LODInfo[] DetailLevels;

        public int ColliderLODIndex;

        public MeshSettings MeshSetting;
        public HeightMapSettings HeightMapSetting;
        public TextureData TextureSetting;
        public BaseSpawnSettings BaseSettings;

        public Transform Viewer;

        public Material MapMaterial;

        private Vector2 _viewerPosition;
        private Vector2 _viewerPositionOld;


        private int _chunkVisibleInViewDist;

        private float _meshWorldSize;

        private readonly Dictionary<Vector2, TerrainChunk> _terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
        private readonly List<TerrainChunk> _vissibleTerrainChunks = new List<TerrainChunk>();

   

        private void Start()
        {
            TextureSetting.ApplyToMaterial(MapMaterial);
            TextureSetting.UpdateMeshHeights(MapMaterial, HeightMapSetting.MinHeight, HeightMapSetting.MaxHeight);

            float maxViewDist = DetailLevels[DetailLevels.Length - 1].VisibleDstTreshold;
            _meshWorldSize = MeshSetting.MeshWorldSize;
            _chunkVisibleInViewDist = Mathf.RoundToInt(maxViewDist /_meshWorldSize);

            UpdateVisibleChunks();

        }

        private void Update()
        {
            _viewerPosition = new Vector2(Viewer.position.x, Viewer.position.z) ;

            if (_viewerPosition != _viewerPositionOld)
            {
                foreach (var chunk in _vissibleTerrainChunks)
                {
                    chunk.UpdateCollisionMesh();
                }
            }

            if (!((_viewerPositionOld - _viewerPosition).sqrMagnitude > SqrViewerMoveTresholdForChunkUpdate)) return;
            _viewerPositionOld = _viewerPosition;
            UpdateVisibleChunks();

        }

        private void UpdateVisibleChunks()
        {

            HashSet<Vector2> alreadyUpdatedChunkCoords = new HashSet<Vector2>();
            for (int i = _vissibleTerrainChunks.Count -1; i >= 0; i--)
            {
                alreadyUpdatedChunkCoords.Add(_vissibleTerrainChunks[i].Coord);
                _vissibleTerrainChunks[i].UpdateTerrainChunk();
            }

        

            int currentChunkCoordX = Mathf.RoundToInt(_viewerPosition.x / _meshWorldSize);
            int currentChunkCoordY = Mathf.RoundToInt(_viewerPosition.y / _meshWorldSize);


            for (int yOffset = -_chunkVisibleInViewDist; yOffset <= _chunkVisibleInViewDist; yOffset++)
            {
                for (int xOffset = -_chunkVisibleInViewDist; xOffset <= _chunkVisibleInViewDist; xOffset++)
                {
                    Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
                    if (alreadyUpdatedChunkCoords.Contains(viewedChunkCoord)) continue;
                    if (_terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                    {
                        _terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();

                    }
                    else
                    {
                        TerrainChunk newChunk = new TerrainChunk(viewedChunkCoord, HeightMapSetting,MeshSetting, BaseSettings, DetailLevels, ColliderLODIndex, transform,Viewer, MapMaterial);
                        _terrainChunkDictionary.Add(viewedChunkCoord, newChunk);
                        newChunk.OnVisibilityChanged += OnTerrainChunkVisibiltyChanged;
                        newChunk.Load();
                    }
                }
            }


        }

        private void OnTerrainChunkVisibiltyChanged(TerrainChunk chunk, bool isVisible )
        {
            if (isVisible)
            {
                _vissibleTerrainChunks.Add(chunk);
            }
            else
            {
                _vissibleTerrainChunks.Remove(chunk);
            }
        }
    }
}


