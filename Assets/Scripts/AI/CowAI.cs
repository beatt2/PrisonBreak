using System.Collections;
using Items;
using UnityEngine;
using UnityStandardAssets.Effects;

namespace AI
{
    public class CowAI : _AI
    {

        private int _destinationIndex;
        private DynamiteVisual _dynamiteVisual;
        private GameObject _afterBurner;

        public enum StageEnum
        {
            Field, ToButcher, AtButcher, Butcher 
        }
        public StageEnum CurrentStage;


        protected override void Awake()
        {
            _dynamiteVisual = GetComponent<DynamiteVisual>();
            _afterBurner = GetComponentInChildren<AfterburnerPhysicsForce>().gameObject;
            _afterBurner.SetActive(false);
            base.Awake();
        
        }
  
        private void Start()
        {
            CowManager.Instance.Add(CowManager.CowLocationEnum.Field,this);
            NavMeshAgent.updateUpAxis = true;
            NavMeshAgent.stoppingDistance = 0.2f;
            CurrentStage = StageEnum.Field;    
        }

        public void ActivateAfterBurner(bool active)
        {
            _afterBurner.SetActive(active);
        }

        public bool Booming()
        {
            return _afterBurner.activeSelf;
        }



        protected override void Update()
        {
            if (!_afterBurner.activeSelf) return;
            var destination = GuardManager.Instance.GetRandomPosition(GuardManager.LocationEnum.Butchery);
            NavMeshAgent.SetDestination(destination);
        }

        public void ChangeNavMesh()
        {
            NavMeshAgent.speed = 5;
            NavMeshAgent.angularSpeed = 50;
            NavMeshAgent.acceleration = 10;
            NavMeshAgent.stoppingDistance = 0;
            NavMeshAgent.autoBraking = false;
            ActivateAfterBurner(true);
           
        }

        public void ActivatedNavMesh()
        {
            NavMeshAgent.enabled = true;
        }


        public void ToNextDestination()
        {
            if (CurrentStage == StageEnum.Butcher)
            {
                NavMeshAgent.stoppingDistance = 0;
            }

            NavMeshAgent.SetDestination(LocationAI.Instance.GetPositionCow(_destinationIndex));
            _destinationIndex++;
      
        }

        public void ButcherDistanceCheck()
        {
            StartCoroutine(CheckDistance());
        }
    

        private IEnumerator CheckDistance()
        {
            yield return new WaitForSeconds(3);
            bool reached = false;
            while (!reached)
            {
              
                if (NavMeshAgent.remainingDistance < 0.2f)
                {
                    CurrentStage = StageEnum.AtButcher;
                    CowManager.Instance.CowReachedDestination(this);
                    reached = true;
                }
                yield return null;
            }
        }

        public void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Guard") || !Booming()) return;
            other.gameObject.GetComponent<GuardAI>().Health -= 100;
            GuardManager.Instance.RemoveFromList(GuardManager.LocationEnum.Butchery,other.gameObject.GetComponent<GuardAI>());
            CowManager.Instance.RemoveCow(gameObject);
            GuardManager.Instance.TriggerGuards(GuardManager.LocationEnum.Butchery);
            EffectManager.Instance.InstansiateExplosion(transform);
            Destroy(gameObject);
        }


    }
}
