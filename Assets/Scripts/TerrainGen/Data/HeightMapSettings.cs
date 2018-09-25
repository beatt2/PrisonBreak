using UnityEngine;

namespace TerrainGen.Data
{

    [CreateAssetMenu]
    public class HeightMapSettings : UpdateableData
    {
        public Noise.NoiseSettings NoiseSetting;

       

        public float HeightMultiplier;
        public AnimationCurve HeightCurve;
        public bool UseFallOff;


        public float MinHeight => HeightMultiplier * HeightCurve.Evaluate(0);
        public float MaxHeight => HeightMultiplier * HeightCurve.Evaluate(1);


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            NoiseSetting.ValidateValues();
            base.OnValidate();
           
        }
#endif
    }

}
