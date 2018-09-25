using TerrainGen.Data;
using UnityEngine;

namespace TerrainGen
{
    public static class HeightMapGenerator
    {

        public static HeightMap GenerateHeightMap(int width, int height, HeightMapSettings settings, BaseSpawnSettings baseSpawnSettings, Vector2 sampleCentre)
        {
            float[,] values = Noise.GenerateNoiseMap(width, height, settings.NoiseSetting, sampleCentre);
            // ReSharper disable once InconsistentNaming
            AnimationCurve heightCurve_threadsafe = new AnimationCurve(settings.HeightCurve.keys);

            float minValue = float.MaxValue;
            float maxValue = float.MinValue;
            float[,] fallOfMap = { };
            if (settings.UseFallOff)
            {
                fallOfMap = FallOffGenerator.GenrateFallOfMap(width);
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {

                    if (settings.UseFallOff)
                    {
                        values[i, j] = values[i, j] - fallOfMap[i, j];
                    }

                    values[i, j] *= heightCurve_threadsafe.Evaluate(values[i, j]) * settings.HeightMultiplier;

                    if (values[i, j] > maxValue)
                    {
                        maxValue = values[i, j];

                    }

                    if (values[i, j] < minValue)
                    {
                        minValue = values[i, j];


                    }

                }

            }

            if (!baseSpawnSettings.UseBaseSpawn) return new HeightMap(values, minValue, maxValue);
            {
                BaseSpawn baseSpawn = new BaseSpawn(0.2f, 0.326f, values, minValue, maxValue);
                if (values[baseSpawn.TargetCoords.x, baseSpawn.TargetCoords.y] < baseSpawn.GetTarget())
                {
                    values[baseSpawn.TargetCoords.x, baseSpawn.TargetCoords.y] = baseSpawn.GetTarget();
                }
                baseSpawnSettings.SetSpawnLocation(new Vector3(
                    baseSpawn.TargetCoords.x,
                
                    values[baseSpawn.TargetCoords.x, baseSpawn.TargetCoords.y],
                    baseSpawn.TargetCoords.y
                ));

                float[,] reversedFallOff = ReversedFallOfMap.GenerateReversedFallOfMap(width, baseSpawn.TargetCoords);

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (!(reversedFallOff[i, j] < 30)) continue;
                        if (reversedFallOff[i, j] < 4f)
                        {
                            if (values[baseSpawn.TargetCoords.x, baseSpawn.TargetCoords.y] < baseSpawn.GetTarget())
                            {
                                values[i, j] = baseSpawn.GetTarget();
                            }
                            else
                            {
                                values[i, j] = values[baseSpawn.TargetCoords.x, baseSpawn.TargetCoords.y];
                            }
                     


                        }
                        else if (values[i, j] > values[baseSpawn.TargetCoords.x, baseSpawn.TargetCoords.y] && reversedFallOff[i, j] < 7)
                        {
               
                            values[i, j] = values[i, j] - 4;
                            if (values[i, j] <= values[baseSpawn.TargetCoords.x, baseSpawn.TargetCoords.y])
                            {
                                values[i, j] = values[baseSpawn.TargetCoords.x, baseSpawn.TargetCoords.y];

                            }
                        }
                        else if (values[i, j] > values[baseSpawn.TargetCoords.x, baseSpawn.TargetCoords.y] && reversedFallOff[i, j] < 10)
                        {
                     
                            values[i, j] = values[i, j] - 3;
                            if (values[i, j] <= values[baseSpawn.TargetCoords.x, baseSpawn.TargetCoords.y])
                            {
                                values[i, j] = values[baseSpawn.TargetCoords.x, baseSpawn.TargetCoords.y] + Random.Range(0, 0.5f);

                            }
                        }
                        else if (values[i, j] > values[baseSpawn.TargetCoords.x, baseSpawn.TargetCoords.y] && reversedFallOff[i, j] < 12)
                        {
                            var temp = values[i, j];
                            values[i, j] = values[i, j] - 2f;
                            if (values[i, j] <= values[baseSpawn.TargetCoords.x, baseSpawn.TargetCoords.y])
                            {
                                values[i, j] = temp;

                            }
                        }
                        else if (values[i, j] > values[baseSpawn.TargetCoords.x, baseSpawn.TargetCoords.y] && reversedFallOff[i, j] < 15)
                        {
                            var temp = values[i, j];
                            values[i, j] = values[i, j] - 1;
                            if (values[i, j] <= values[baseSpawn.TargetCoords.x, baseSpawn.TargetCoords.y])
                            {
                                values[i, j] = temp;

                            }

                        }
                    }

                }
            }

            return new HeightMap(values, minValue, maxValue);


        }





    }

    public struct HeightMap
    {
        public readonly float[,] Values;
        public readonly float MinValue;
        public readonly float MaxValue;

        public HeightMap(float[,] values, float minValue, float maxValue)
        {

            Values = values;
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}