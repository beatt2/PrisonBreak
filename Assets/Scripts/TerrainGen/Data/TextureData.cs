using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

namespace TerrainGen.Data
{
    [CreateAssetMenu]
    public class TextureData : UpdateableData
    {
        private const int TextureSize = 512;
        private const TextureFormat MyTextureFormat = TextureFormat.RGB565;

        public Layer[] Layers;
 
        private float _savedMinHeight;
        private float _savedMaxHeight;


        public void ApplyToMaterial(Material material)
        {
         
            UpdateMeshHeights(material, _savedMinHeight, _savedMaxHeight);
        }


        public void UpdateMeshHeights(Material material, float minHeight, float maxHeight)
        {
            _savedMinHeight = minHeight;
            _savedMaxHeight = maxHeight;

  
            material.SetFloat("minHeight", minHeight);
            material.SetFloat("maxHeight", maxHeight);
            material.SetInt("layerCount", Layers.Length);
            material.SetColorArray("baseColors", Layers.Select(x => x.Tint).ToArray());
            material.SetFloatArray("baseStartHeights", Layers.Select(x => x.StartHeight).ToArray());
            material.SetFloatArray("baseBlends", Layers.Select(x => x.BlendStrength).ToArray());
            material.SetFloatArray("baseColorStrength", Layers.Select(x => x.TintStrength).ToArray());
            material.SetFloatArray("baseTextureScales", Layers.Select(x => x.TextureScale).ToArray());
            Texture2DArray texturesArray = GenerateTextureArray(Layers.Select(x => x.Texture).ToArray());
            material.SetTexture("baseTextures", texturesArray);

        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private static Texture2DArray GenerateTextureArray(Texture2D[] textures)
        {
            Texture2DArray textureArray = new Texture2DArray(TextureSize,TextureSize, textures.Length, MyTextureFormat,true);
            for (int i = 0; i < textures.Length; i++)
            {
                textureArray.SetPixels(textures[i].GetPixels(), i);
            }
            textureArray.Apply();
            return textureArray;

        }

        [Serializable]
        [SuppressMessage("ReSharper", "UnassignedField.Global")]
        public class Layer
        {
            public string Name;
            public Texture2D Texture;
            public Color Tint;
            [Range(0,1)]
            public float TintStrength;
            [Range(0,1)]
            public float StartHeight;
            [Range(0,1)]
            public float BlendStrength;
            public float TextureScale;
        }

        [Serializable]
        public class Vegetation
        {
            public float Density;
        }

    }
}
 