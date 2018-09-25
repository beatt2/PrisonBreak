using UnityEngine;

namespace TerrainGen
{
    public static class TextureGenerator 
    {
        private static Texture2D TextureFromColorMap(Color[] colorMap, int widht, int height)
        {
            Texture2D texture = new Texture2D(widht, height)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
            texture.SetPixels(colorMap);
            texture.Apply();
            return texture;
        }

        public static Texture2D TextureFromHeightMap(HeightMap heightMap)
        {
            int width = heightMap.Values.GetLength(0);
            int height = heightMap.Values.GetLength(1);


            Color[] colorMap = new Color[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, Mathf.InverseLerp(heightMap.MinValue, heightMap.MaxValue,heightMap.Values[x,y]));

                }
            }
            return TextureFromColorMap(colorMap, width, height); 
  
        }
    }
}
