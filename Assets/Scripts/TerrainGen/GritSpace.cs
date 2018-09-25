using UnityEngine;

namespace TerrainGen
{
    public static class GritSpace 
    {
        public static Vector3 GritToWorld(Vector3 value)
        {
            Vector3 result = new Vector3();


            if (value.x <= 48)
            {
                float tempval = value.x - 49;
                result.x = tempval * 0.96907216494845360824742268041237f;
            }

            else
            {
                float tempval = 48 - value.x;
                result.x -= tempval * 0.96907216494845360824742268041237f;
            }

            if (value.z <= 48)
            {
                float tempval = 48 - value.z;
                result.z  =  tempval * 0.96907216494845360824742268041237f;
            }
            else
            {
                float tempval = value.z - 49;
                result.z -= tempval* 0.96907216494845360824742268041237f;
            }

            return new Vector3(result.x, value.y, result.z);
        }
    }
}
