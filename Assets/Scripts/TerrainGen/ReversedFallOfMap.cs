using UnityEngine;

namespace TerrainGen
{
    public static class ReversedFallOfMap  
    {

        public static float[,] GenerateReversedFallOfMap(int size, Vector2Int target)
        {

            float [,] values = new float[size,size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
            
                    values[i, j] = Vector2Int.Distance(new Vector2Int(i, j), target);

                }
            }


            return values;
        }

    }
}

