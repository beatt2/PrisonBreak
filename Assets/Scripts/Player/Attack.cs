using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class Attack : MonoBehaviour
    {
        public Slider PowerUpSlider;
        private Rigidbody _rigidbody;

        private bool _go;

        public Text Hint;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            PowerUpSlider.gameObject.SetActive(false);
        }

        private void Update()
        {
            PowerUpSlider.value = Input.GetAxis("Jump");
            if (Input.GetAxis("Jump") < 0.01f && _go)
            {
                _go = false;
                PowerUpSlider.gameObject.SetActive(false);
                Hint.gameObject.SetActive(true);
            }

            if (Input.GetButtonDown("Jump"))
            {
                PowerUpSlider.gameObject.SetActive(true);
                Hint.gameObject.SetActive(false);
            }

            if (!Input.GetButtonUp("Jump")) return;
            _go = true;
        }

        private void FixedUpdate()
        {
            if (_go)
                _rigidbody.AddRelativeForce(Input.GetAxis("Jump") * 5000, 0, 0);
        }
    }
}