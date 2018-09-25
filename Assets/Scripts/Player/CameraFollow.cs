using UnityEngine;


namespace Player
{
    public class CameraFollow : MonoBehaviour
    {
        
        private Vector3 _playerRotation;
        private float _xRotation;

        private void Awake()
        {
          
            _playerRotation = transform.eulerAngles;
            _xRotation = _playerRotation.x;
        }

        public void ChangeParent()
        {
            transform.parent = null;
        }

        private void FixedUpdate()
        {

            transform.eulerAngles = new Vector3(_xRotation,transform.eulerAngles.y,0);
        }
    }
}
