using Door;
using UnityEngine;

namespace Spike
{
    public class Spikes : MonoBehaviour
    {
        public int SpikeID;

        [SerializeField]
        private bool _openTogetherWithDoorID;

        public bool OpensWithDoorID => _openTogetherWithDoorID;


        private LerpCoroutine _lerpCoroutine;

        private void Awake()
        {
            _lerpCoroutine = GetComponent<LerpCoroutine>();
        }

        //go down
        public void Open()
        {
            _lerpCoroutine.Open();
        }

        //go up
        public void Close()
        {
            _lerpCoroutine.Close();

        }

        public int ID()
        {
            return SpikeID;
        }

        private void OnTriggerEnter(Collider other)
        {
            Invoke(nameof(Open), 3);
            Invoke(nameof(Close), 7);
        }
    }
}
