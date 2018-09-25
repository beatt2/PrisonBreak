using System.Collections.Generic;
using UnityEngine;

namespace TerrainGen
{
    public class BaseSpawn
    {
        public List<Vector2Int> NeigbourCoords = new List<Vector2Int>();
        public Vector2Int TargetCoords;


        // ReSharper disable once NotAccessedField.Local
        // ReSharper disable once RedundantDefaultMemberInitializer
        private Vector3 _finalVector3 = new Vector3();
        public void SetFinalVector3(float height)
        {
            _finalVector3 = new Vector3(TargetCoords.x, height, TargetCoords.y);
        }

        private float _target;
        public float GetTarget()
        {
            return _target;
        }

        public BaseSpawn(float startHeight, float endHeight, float[,] values, float minValue, float maxValue)
        {
            TargetCoords = GetBestAverageIndex(startHeight, endHeight, values, minValue, maxValue);
        }

        private Vector2Int GetBestAverageIndex(float startHeight, float endHeight, float[,] values, float minValue, float maxValue)
        {
            Vector2Int results = new Vector2Int();
            List<Vector2Int> baseResultList = new List<Vector2Int>();

            float record = float.MaxValue;
            float finalRecord = float.MaxValue;
            float requestedHeight = (startHeight + endHeight) / 2;
            float target = requestedHeight * (maxValue - minValue) + minValue;
            _target = target;


            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int j = 0; j < values.GetLength(1); j++)
                {


                    float result = Mathf.Abs(target - values[i, j]);


                    if (!(record > result)) continue;
                    record = result;
                    bool[] myB = new bool[8];
                    for (var i1 = 0; i1 < myB.Length; i1++)
                    {
                        myB[i1] = true;
                    }


                    if (i - 1 < 0)
                    {
                        myB[0] = false;
                        myB[3] = false;
                        myB[5] = false;
                    }
                    if (i + 1 >= values.GetLength(0))
                    {
                        myB[2] = false;
                        myB[4] = false;
                        myB[7] = false;
                    }
                    if (j - 1 < 0)
                    {
                        myB[0] = false;
                        myB[1] = false;
                        myB[2] = false;
                    }

                    if (j + 1 >= values.GetLength(1))
                    {
                        myB[5] = false;
                        myB[6] = false;
                        myB[7] = false;
                    }

                    List<Vector2Int> baseMaps = new List<Vector2Int>();


                    for (int k = 0; k < myB.Length; k++)
                    {
                        if (!myB[k]) continue;
                        // ReSharper disable once SwitchStatementMissingSomeCases
                        switch (k)
                        {
                            case 0:
                                baseMaps.Add(new Vector2Int(i - 1, j - 1));
                                break;
                            case 1:
                                baseMaps.Add(new Vector2Int(i, j - 1));
                                break;
                            case 2:
                                baseMaps.Add(new Vector2Int(i + 1, j - 1));
                                break;
                            case 3:
                                baseMaps.Add(new Vector2Int(i - 1, j));
                                break;
                            case 4:
                                baseMaps.Add(new Vector2Int(i + 1, j));
                                break;
                            case 5:
                                baseMaps.Add(new Vector2Int(i - 1, j + 1));
                                break;
                            case 6:
                                baseMaps.Add(new Vector2Int(i, j + 1));
                                break;
                            case 7:
                                baseMaps.Add(new Vector2Int(i + 1, j + 1));
                                break;
                       
                        }
                    }
                    float total = 0;

                    // ReSharper disable once ForCanBeConvertedToForeach
                    // ReSharper disable once LoopCanBeConvertedToQuery
                    for (int k = 0; k < baseMaps.Count; k++)
                    {
                        total += values[baseMaps[k].x, baseMaps[k].y];
                    }



                    float average = total / baseMaps.Count;
                    float diffrence = Mathf.Abs(average - target);
                    if (!(finalRecord > diffrence)) continue;
                    finalRecord = diffrence;
                    baseResultList = baseMaps;
                    results = new Vector2Int(i, j);
                }
            }

            NeigbourCoords = baseResultList;
            return results;

        }
    }
}
