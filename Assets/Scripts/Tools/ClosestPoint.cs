using System;
using System.Linq;
using UnityEngine;

namespace Tools
{
    public static class ClosestPoint
    {

        public static Vector3 GetClosestPositionVector3(Vector3 target, Vector3[] positions)
        {
            var distanceLists = (from pos in positions let tempfloat = Vector3.Distance(target, pos) select new DistanceList(tempfloat, pos)).ToList();
            distanceLists.Sort();
            return distanceLists[0].GetPositionVector3;
        }
        public static  Vector3 GetClosestPositionVector3(Vector3 target, Transform[] positions)
        {
            var distanceLists = (from pos in positions let tempfloat = Vector3.Distance(target, pos.position) select new DistanceList(tempfloat, pos.position)).ToList();
            distanceLists.Sort();
            Debug.Log(distanceLists[0].GetPositionVector3);
            return distanceLists[0].GetPositionVector3;
        }


    }

    public class DistanceList : IComparable<DistanceList>
    {
        public Vector3 GetPositionVector3 { get; private set; }
        public float GetDistanceToPlayer { get; private set; }

        public int CompareTo(DistanceList other)
        {
            if (other == null)
            {
                return 1;
            }

            if (GetDistanceToPlayer > other.GetDistanceToPlayer)
            {
                return 1;
            }
            else if (GetDistanceToPlayer < other.GetDistanceToPlayer)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }


        public DistanceList(float distance, Vector3 positionVector3)
        {
            GetPositionVector3 = positionVector3;
            GetDistanceToPlayer = distance;
        }

    }
}
