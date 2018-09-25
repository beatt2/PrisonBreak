using MathExt;
using Tools;
using UnityEngine;

namespace AI
{
    public class LocationAI : Singleton<LocationAI>
    {
        public Transform[] PositionsCow;
        public Transform[] FencePath;
        public Transform[] Field;
        public Transform Crusher;


        public Vector3 GetPositionCow(int index)
        {
            return PositionsCow[index].position;
        }

        public Vector3 GetClosestPositionFence(Vector3 position)
        {
            return ClosestPoint.GetClosestPositionVector3(position, FencePath);
        }

        public int GetFenceVector3Index(Vector3 position)
        {
            for (int i = 0; i < FencePath.Length; i++)
            {
                if (FencePath[i].position == position)
                {
                    return i;
                }
            }
            throw new System.IndexOutOfRangeException();
        }

        public Vector3 GetRandomLocation(Transform[] transforms)
        {
            return transforms.GetRandom_Array().position;
        }
    }
}
