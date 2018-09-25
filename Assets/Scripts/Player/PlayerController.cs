using AI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {

        public float WalkSpeed;
        public float TurnSpeed;

        public float BoostTime;
        public float BoostSpeed;

        private Rigidbody _rigidbody;

        private float _health = 1;

        public float Health
        {
            get { return _health; }
            set
            {

                _health = value;
                HealthSlider.value = _health;
                if (_health <= 0)
                {
                    Die();
                }
            }
        }

        public Slider HealthSlider;

        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }


        private void FixedUpdate()
        {
            transform.Translate(Input.GetAxis("Vertical") * WalkSpeed,0,0) ;
            float horizon = Input.GetAxis("Horizontal") * TurnSpeed;
            transform.Rotate(0, horizon, 0);



        }

        public void Die()
        {
            EffectManager.Instance.InstansiateExplosion(transform);
            Camera.main.gameObject.GetComponent<CameraFollow>().ChangeParent();
            gameObject.SetActive(false);
            Invoke(nameof(LoadScene), 2);
           
        }

        public void BlowUp()
        {
            _rigidbody.AddForce(0,500000,0);
            Invoke(nameof(Die), 2);
        }

        private void LoadScene()
        {
            SceneManager.LoadScene("MainMenu");

        }
    

        private void OnCollisionEnter(Collision other)
        {
            var behaviour = other.gameObject.GetComponent<IGuardBehaviour>();
            if (behaviour?.GetEnum() == GuardManager.GuardState.Triggerd)
            {
                Health -= behaviour.GetDamage();
            }
        }
    }
}
