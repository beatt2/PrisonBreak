using UnityEngine;

namespace AI
{
    public class GuardManager : AIManagement<GuardAI, GuardManager.LocationEnum, GuardManager>
    {
        public enum LocationEnum
        {
            Butchery = 0, Gatehouse = 1
        }

        public Transform  []SpawnLocationsButchery;
        public Transform[] SpawnLocationGatehouse;
        public GameObject Guard;
        public GameObject GuardPoint;

        public enum GuardState
        {
            Chill, Triggerd
        }

        public void Start()
        {
            foreach (var t in SpawnLocationsButchery)
            {
                GameObject go = Instantiate(Guard, t.position, Quaternion.identity, GuardPoint.transform);
                go.GetComponent<GuardAI>().SetLocationEnum(LocationEnum.Butchery);
            }

            foreach (var t in SpawnLocationGatehouse)
            {
                GameObject go = Instantiate(Guard, t.position, Quaternion.identity, GuardPoint.transform);
                go.GetComponent<GuardAI>().SetLocationEnum(LocationEnum.Gatehouse);
            }
        }

        public void TriggerGuards(LocationEnum location)
        {
            foreach (var guard in  GetList(location))
            {
                guard.SetState(GuardState.Triggerd);
            }
        }

    }
}
