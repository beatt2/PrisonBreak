using System;
using TerrainGen.Data;
using UnityEngine;

namespace TerrainGen
{
    public class TerrainChunk
    {
        private const float ColliderGenerationDistanceTreshold = 5f;
        public event Action<TerrainChunk, bool> OnVisibilityChanged;
        public Vector2 Coord;

        private readonly GameObject _meshObject;
        private readonly Vector2 _sampleCenter;

        private Bounds _bounds;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly MeshRenderer _meshRenderer;
        private readonly MeshFilter _meshFilter;
        private readonly MeshCollider _meshCollider;




        private readonly LODInfo[] _detailLevels;
        private readonly LODMesh[] _lodMeshes;
        private readonly int _collisionLODIndex;

        private HeightMap _heightMap;
        private bool _heightMapReceived;
        private bool _hasSetCollider;

        private int _previousLODIndex = -1;
        private readonly float _maxViewDistance;


        private readonly HeightMapSettings _heightMapSettings;
        private readonly MeshSettings _meshSettings;
        private readonly BaseSpawnSettings _baseSpawnSettings;

        private readonly Transform _viewer;


        public TerrainChunk(Vector2 coord, HeightMapSettings heightMapSettings, MeshSettings meshSettings, BaseSpawnSettings baseSpawnSettings, LODInfo[] detailLevels, int colliderLODIndex, Transform parent,Transform viewer, Material material)
        {

            Coord = coord;
            _detailLevels = detailLevels;
            _collisionLODIndex = colliderLODIndex;
            _heightMapSettings = heightMapSettings;
            _meshSettings = meshSettings;
            _baseSpawnSettings = baseSpawnSettings;
            _viewer = viewer;


            _sampleCenter = coord * meshSettings.MeshWorldSize / meshSettings.MeshScale;
            Vector2 position = coord * meshSettings.MeshWorldSize;
            _bounds = new Bounds(position, Vector2.one * meshSettings.MeshWorldSize);


            _meshObject = new GameObject("Terrain Chunk");

            _meshRenderer = _meshObject.AddComponent<MeshRenderer>();
            _meshFilter = _meshObject.AddComponent<MeshFilter>();
            _meshCollider = _meshObject.AddComponent<MeshCollider>();
            _meshRenderer.material = material;


            _meshObject.transform.position = new Vector3(position.x, 0, position.y);
            _meshObject.transform.parent = parent;
            SetVisible(false);


            _lodMeshes = new LODMesh[detailLevels.Length];
            for (int i = 0; i < detailLevels.Length; i++)
            {
                _lodMeshes[i] = new LODMesh(detailLevels[i].Lod, UpdateTerrainChunk);
                _lodMeshes[i].UpdateCallBack += UpdateTerrainChunk;
                if (i == _collisionLODIndex)
                {
                    _lodMeshes[i].UpdateCallBack += UpdateCollisionMesh;
                }

            }

            _maxViewDistance = detailLevels[detailLevels.Length - 1].VisibleDstTreshold;



        }

        public void Load()
        {
            ThreadedDataRequester.RequestData(() => HeightMapGenerator.GenerateHeightMap(_meshSettings.NumVertsPerLine, _meshSettings.NumVertsPerLine, _heightMapSettings, _baseSpawnSettings, _sampleCenter), OnHeightMapReceived);

        }

        private void OnHeightMapReceived(object heightMapObject)
        {
            _heightMap = (HeightMap)heightMapObject;
            _heightMapReceived = true;

            UpdateTerrainChunk();
        }

        private Vector2 ViewerPosition => new Vector2(_viewer.position.x,_viewer.position.z);


        public void UpdateTerrainChunk()
        {
            if (!_heightMapReceived) return;
            float viewerDstFromNearestEdge = Mathf.Sqrt(_bounds.SqrDistance(ViewerPosition));

            bool wasVisible = IsVisible();
            bool visible = viewerDstFromNearestEdge <= _maxViewDistance;


            if (visible)
            {
                int lodIndex = 0;
                for (int i = 0; i < _detailLevels.Length - 1; i++)
                {

                    if (viewerDstFromNearestEdge > _detailLevels[i].VisibleDstTreshold)
                    {
                        lodIndex = i + 1;
                    }
                    else
                    {
                        break;
                    }
                }

                if (lodIndex != _previousLODIndex)
                {
                    LODMesh lodMesh = _lodMeshes[lodIndex];
                    if (lodMesh.HasMesh)
                    {

                        _previousLODIndex = lodIndex;
                        _meshFilter.mesh = lodMesh.MeshLOD;
                    }
                    else if (!lodMesh.HasRequestedMesh)
                    {


                        lodMesh.RequestMesh(_heightMap, _meshSettings);
                    }
                }
            }

            if (wasVisible == visible) return;
            SetVisible(visible);
            OnVisibilityChanged?.Invoke(this, visible);


        }

        public void UpdateCollisionMesh()
        {
            if (_hasSetCollider) return;
            float sqrDistanceFromViewerToEdge = _bounds.SqrDistance(ViewerPosition);
            if (sqrDistanceFromViewerToEdge < _detailLevels[_collisionLODIndex].SqrVissibleDstTreshold)
            {
                if (!_lodMeshes[_collisionLODIndex].HasRequestedMesh)
                {
                    _lodMeshes[_collisionLODIndex].RequestMesh(_heightMap, _meshSettings);
                }
            }


            if (!(sqrDistanceFromViewerToEdge <
                  ColliderGenerationDistanceTreshold * ColliderGenerationDistanceTreshold)) return;
            if (!_lodMeshes[_collisionLODIndex].HasMesh) return;
            _meshCollider.sharedMesh = _lodMeshes[_collisionLODIndex].MeshLOD;
            _hasSetCollider = true;
        }


        private void SetVisible(bool vissible)
        {
            _meshObject.SetActive(vissible);
        }

        private bool IsVisible()
        {
            return _meshObject.activeSelf;
        }
    }

    internal class LODMesh
    {
        public Mesh MeshLOD;
        public bool HasRequestedMesh;
        public bool HasMesh;

        private readonly int _lod;

        public event Action UpdateCallBack;

        public LODMesh(int lod, Action updateCallBack)
        {
            _lod = lod;

        }


        private void OnMeshDataReceived(object meshDataObject)
        {

            MeshLOD = ((MeshData)meshDataObject).CreateMesh();
            HasMesh = true;
            UpdateCallBack?.Invoke();
        }

        public void RequestMesh(HeightMap heightMap, MeshSettings meshSettings)
        {
            HasRequestedMesh = true;
            ThreadedDataRequester.RequestData(() => MeshGenerator.GenerateTerrainMesh(heightMap.Values, meshSettings, _lod), OnMeshDataReceived);

        }
    }
    [Serializable]
    public struct LODInfo
    {
        [Range(0, MeshSettings.NumSupportedLODs - 1)]
        public int Lod;
        // ReSharper disable once UnassignedField.Global
        public float VisibleDstTreshold;

        public float SqrVissibleDstTreshold => VisibleDstTreshold * VisibleDstTreshold;
    }
}