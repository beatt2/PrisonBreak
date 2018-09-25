using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class LegMove : MonoBehaviour
    {
        public GameObject[] FrontLegs;
        public GameObject[] HindLegs;


        private readonly Quaternion[] _debugMemory = new Quaternion[4];
        private float _moveSpeed;
        private bool _hitAngleTarget = true;
        private readonly List<GameObject> _legs = new List<GameObject>();
        public int WalkSpeed;


        public virtual void Awake()
        {

            _legs.Add(FrontLegs[0]);
            _legs.Add(FrontLegs[1]);
            _legs.Add(HindLegs[0]);
            _legs.Add(HindLegs[1]);
            for (var i = 0; i < 4; i++)
            {
                _debugMemory[i] = _legs[i].transform.localRotation;
            }
        }

        private void Update()
        {


            _moveSpeed = GetAxis() * WalkSpeed;

            if (GetAxis() > 0.1f)
            {

                MoveLegs();
            }
            else if (GetAxis() < -0.1f)
            {
                
                MoveLegs();
            }
            else
            {
                StopLegs();
                _hitAngleTarget = false;
            }
        }

        //250&290
        private void StopLegs()
        {
            for (var i = 0; i < 4; i++)
            {
                _legs[i].transform.localRotation = _debugMemory[i];
            }
        }


        public virtual float GetAxis()
        {
            return Input.GetAxis("Vertical");
        }

        private void MoveLegs()
        {
            if (_hitAngleTarget && FrontLegs[0].transform.localRotation.x <= -.30f)
            {

                FrontLegs[0].transform.Rotate(_moveSpeed, 0, 0, Space.Self);
                FrontLegs[1].transform.Rotate(-_moveSpeed, 0, 0, Space.Self);
                HindLegs[0].transform.Rotate(-_moveSpeed, 0, 0, Space.Self);
                HindLegs[1].transform.Rotate(_moveSpeed, 0, 0, Space.Self);

            }
            else if (_hitAngleTarget)
            {
                _hitAngleTarget = false;
            }

            else if (!_hitAngleTarget && FrontLegs[0].transform.localRotation.x >= -.65f)
            {
                FrontLegs[0].transform.Rotate(-_moveSpeed, 0, 0, Space.Self);
                FrontLegs[1].transform.Rotate(_moveSpeed, 0, 0, Space.Self);
                HindLegs[0].transform.Rotate(_moveSpeed, 0, 0, Space.Self);
                HindLegs[1].transform.Rotate(-_moveSpeed, 0, 0, Space.Self);
            }
            else if (!_hitAngleTarget)
            {
                _hitAngleTarget = true;
            }
        }



    }
}
