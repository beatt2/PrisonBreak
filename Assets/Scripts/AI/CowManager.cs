using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class CowManager : AIManagement<CowAI,CowManager.CowLocationEnum,CowManager>
    {

        private readonly List<CowAI> _readyCows = new List<CowAI>();

        public GameObject Cow;
        public GameObject CowPoint;
        public Transform[] SpawnLocations;
        public enum CowLocationEnum
        {  
            Field
        }

        public void Start()
        {
            foreach (var loc in SpawnLocations)
            {
                Instantiate(Cow, loc.position, Quaternion.identity, CowPoint.transform);
            }

            Invoke(nameof(DelayedStart), 5);
        }

        private void DelayedStart()
        {
            SendSomeCowsToPath();
        }

        private void SendSomeCowsToPath()
        {
            StartCoroutine(PatrolTransforms(CowLocationEnum.Field, 3, LocationAI.Instance.Field, 20));
        }

        public void CowReachedDestination(CowAI cow)
        {
            _readyCows.Add(cow);
            if (cow.CurrentStage != CowAI.StageEnum.AtButcher) return;
            cow.CurrentStage = CowAI.StageEnum.AtButcher;
            if (_readyCows.Count == 1)
            {
                ButcherCow();
            }
        }

        public void RemoveCow(GameObject go)
        {
            _readyCows.Remove(go.GetComponent<CowAI>());
            RemoveFromList(CowLocationEnum.Field, go.GetComponent<CowAI>());

        }

        public void SetAllCowsToButcher()
        {
            StopAllCoroutines();
            foreach (var cow in GetList(CowLocationEnum.Field))
            {
                SendAllToPosition(CowLocationEnum.Field, LocationAI.Instance.GetPositionCow(0));
                cow.CurrentStage = CowAI.StageEnum.ToButcher;
                cow.ButcherDistanceCheck();
            }
        }

        private void ButcherCow()
        {
            if (_readyCows.Count <= 0)
            {
                GuardManager.Instance.TriggerGuards(GuardManager.LocationEnum.Butchery);
                return;
            }
            _readyCows[0].StoppingDistance = 0;
            _readyCows[0].CurrentStage = CowAI.StageEnum.Butcher;
            _readyCows[0].SendToLocation(LocationAI.Instance.Crusher.position);
        }

        public void FinishedButchering()
        {
            Destroy(_readyCows[0].gameObject);
            RemoveFromList(CowLocationEnum.Field, _readyCows[0]);
            _readyCows.RemoveAt(0);
            Invoke(nameof(ButcherCow), 10);

        }
    }
}
