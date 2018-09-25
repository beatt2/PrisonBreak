using Items;
using UnityEngine.UI;

namespace AI
{
    public class GuardAI : _AI , IGuardBehaviour
    {
       
        private int _currentIndex;
        private const float MaxDistance = 1.5f;


        private float _health = 1;
        public float Health
        {
            get { return _health; }
            set
            {             
                _health = value;
                HealthSlider.value = _health;
                if (!(_health <= 0)) return;
                InventoryManager.Instance.InstansiateAccessItem(transform);
                Destroy(gameObject);
            }
        }
        public Slider HealthSlider;

        public GuardManager.LocationEnum Location;
        public GuardManager.GuardState GuardState;

        private GuardAttack _guardAttack;
     
        //Interface
        public GuardManager.GuardState GetEnum()
        {
            return GuardState;
        }

        protected override void Awake()
        {
            _guardAttack = GetComponent<GuardAttack>();
            base.Awake();
        }
        
        private const float Damage = 0.05f;
        public float GetDamage()
        {
            return Damage;
        }

        private void Start()
        {
            GuardManager.Instance.Add(Location, this);
  
            GuardState = GuardManager.GuardState.Chill;
        }

        public void SetLocationEnum(GuardManager.LocationEnum type)
        {
            Location = type;
        }

        private void InitFence()
        {
            var tempLoc = LocationAI.Instance.GetClosestPositionFence(transform.position);
            _currentIndex = LocationAI.Instance.GetFenceVector3Index(tempLoc);
            NavMeshAgent.SetDestination(tempLoc);
        }

        
        public void SetState(GuardManager.GuardState state)
        {
            GuardState = state;
            if (GuardState != GuardManager.GuardState.Triggerd) return;
            SendToLocation(GameController.Instance.GetPlayerLocation, true);
            _guardAttack.StateHasChanged(state);
        }

  
    }
}
