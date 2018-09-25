using System;
using AI;
using UnityEngine;

namespace Items
{
    public class DynamiteItem : Item
    {
        public float TimeTillExplosion { get; }
        private DynamiteVisual _dynamiteVisual;

        public enum WhereAmIEnum
        {
            Player, Cow, Ground
        }

        public WhereAmIEnum WhereAmI;


        public DynamiteItem(string name, int weight, float timeTillExplosion) : base(name, weight)
        {
            TimeTillExplosion = timeTillExplosion;
            WhereAmI = WhereAmIEnum.Player;
        }

        private void StartTimer()
        {
            InventoryManager.Instance.StartTimer(TimeTillExplosion);
        }

        public void TimerFinished()
        {
            if (WhereAmI == WhereAmIEnum.Player)
            {
                GameController.Instance.Player.Die();
            }

            if (WhereAmI != WhereAmIEnum.Cow) return;
            _dynamiteVisual.gameObject.GetComponent<CowAI>().ChangeNavMesh();
            //InventoryManager.Instance.RemoveItem(this);
        }

        public void TransferToOtherCow(GameObject cow)
        {
            _dynamiteVisual.Disable();
            _dynamiteVisual = cow.GetComponent<DynamiteVisual>();
            WhereAmI = WhereAmIEnum.Cow;
            CowManager.Instance.RemoveCow(cow);
            _dynamiteVisual.Enable();

        }

        public void StartTimerIfValid(Collider other)
        {
            if (other.gameObject.GetComponent<DynamiteVisual>() != null)
            {
                _dynamiteVisual = other.gameObject.GetComponent<DynamiteVisual>();
                _dynamiteVisual.Enable();
                StartTimer();
            }
            else
            {

                throw new NullReferenceException();
            }
  
        }
    }
}
