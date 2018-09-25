using System;
using TerrainGen.Data;
using UnityEngine;
using UnityEngine.AI;

namespace TerrainGen
{
    public class MapPreview : MonoBehaviour
    {
        public Renderer TextureRenderer;
        public MeshFilter MyMeshFilter;
        public Material TerrainMaterial;

        public enum DrawModeEnum {NoiseMap, DrawMesh, FallofMap, ReversedFallOfMap}
        public DrawModeEnum DrawMode;

        public MeshSettings MyMeshSettings;
        public HeightMapSettings MyHeightMapSettings;
        public TextureData MyTextureData;
        public BaseSpawnSettings MyBaseSpawnSettings;
        private ObjectSpawner _objectSpawner;


        public MeshCollider MeshCollider;


        [Range(0, MeshSettings.NumSupportedLODs - 1)]
        public int EditorPreviewLOD;


        public bool AutoUpdate;
   

        private BaseSpawner _baseSpawner;


        private void Awake()
        {
            MyHeightMapSettings.NoiseSetting.Seed = UnityEngine.Random.Range(0, 300);
            DrawMapInEditor();
        }
    

        public void DrawMapInEditor()
        {
            _objectSpawner = GetComponent<ObjectSpawner>();
            _baseSpawner = GetComponent<BaseSpawner>();
            MyTextureData.ApplyToMaterial(TerrainMaterial);
            MyTextureData.UpdateMeshHeights(TerrainMaterial, MyHeightMapSettings.MinHeight, MyHeightMapSettings.MaxHeight);


            HeightMap heightMap = HeightMapGenerator.GenerateHeightMap(MyMeshSettings.NumVertsPerLine, MyMeshSettings.NumVertsPerLine, MyHeightMapSettings,MyBaseSpawnSettings, Vector2.zero);
     
            switch (DrawMode)
            {
                case DrawModeEnum.NoiseMap:
                    DrawTexture(TextureGenerator.TextureFromHeightMap(heightMap));
                    break;
                case DrawModeEnum.DrawMesh:
                    DrawMesh(MeshGenerator.GenerateTerrainMesh(heightMap.Values, MyMeshSettings, EditorPreviewLOD));
                    break;
                case DrawModeEnum.FallofMap:
                    DrawTexture(
                        TextureGenerator.TextureFromHeightMap(new HeightMap(FallOffGenerator.GenrateFallOfMap(MyMeshSettings.NumVertsPerLine),0,1)));
                    break;
                case DrawModeEnum.ReversedFallOfMap:
                    DrawTexture(TextureGenerator.TextureFromHeightMap(new HeightMap(ReversedFallOfMap.GenerateReversedFallOfMap(MyMeshSettings.NumVertsPerLine,new Vector2Int(21,44)),0,1)));
                    break;
               
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _baseSpawner.SpawnBase(MyBaseSpawnSettings.SpawnLocation);

        
        }




        private void OnValidate()
        {
            if (MyMeshSettings != null)
            {
                MyMeshSettings.OnValueUpdated -= OnValuesUpdate;
                MyMeshSettings.OnValueUpdated += OnValuesUpdate;
            }

            if (MyHeightMapSettings != null)
            {
                MyHeightMapSettings.OnValueUpdated -= OnValuesUpdate;
                MyHeightMapSettings.OnValueUpdated += OnValuesUpdate;
            }

            if (MyTextureData == null) return;
            MyTextureData.OnValueUpdated -= OnTextureValuesUpdate;
            MyTextureData.OnValueUpdated += OnTextureValuesUpdate;
        }


        private void DrawTexture(Texture texture)
        {
            TextureRenderer.sharedMaterial.mainTexture = texture;
            TextureRenderer.transform.localScale = new Vector3(texture.width,1,texture.height)/10f;

            TextureRenderer.gameObject.SetActive(true);
            MyMeshFilter.gameObject.SetActive(false);
        }

        private void DrawMesh(MeshData meshData)
        {
            MyMeshFilter.sharedMesh = meshData.CreateMesh();
            TextureRenderer.gameObject.SetActive(false);
            MyMeshFilter.gameObject.SetActive(true);
        
            AddCollider();
            AddObjects(meshData);
        }

        private void AddCollider()
        { 

            MeshCollider.sharedMesh = MyMeshFilter.sharedMesh;
            MyMeshFilter.gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();

        }

        private void AddObjects(MeshData meshData)
        {
            _objectSpawner.SpawnObjects(meshData);
        }

        private void OnValuesUpdate()
        {
            if (!Application.isPlaying)
            {
                //DrawMapInEditor();
            }
        }

        private void OnTextureValuesUpdate()
        {
            MyTextureData.ApplyToMaterial(TerrainMaterial);
        }

    }
}
