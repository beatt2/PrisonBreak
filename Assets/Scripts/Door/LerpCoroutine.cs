using System.Collections;
using UnityEngine;

namespace Door
{
    public class LerpCoroutine : MonoBehaviour
    {

        public enum ModeEnum
        {
            Spike, Cylinder
        }

        public ModeEnum Mode;

        public Vector3 Target;
        public float LerpSpeed;
        public float WaitForSecondsOpenFloat;
        public float WaitForSecondsCloseFloat;

        private Vector3 _orignal;

        private void Start()
        {
            _orignal = transform.localPosition;
        }

        public void Open()
        {
            StopAllCoroutines();
            StartCoroutine(OpenDoorIEnumerator());
        }

        public void Close()
        {
            StartCoroutine(CloseDoorIEnumerator());
        }
        private IEnumerator OpenDoorIEnumerator()
        {
            yield return new WaitForSeconds(WaitForSecondsOpenFloat);
            bool moving = true;
            while (moving)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, Target, LerpSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.localPosition, Target) < 0.1f)
                {
                    moving = false;
                }
    
                yield return null;

            }
        }

        private IEnumerator CloseDoorIEnumerator()
        {
            yield return new WaitForSeconds(WaitForSecondsCloseFloat);
            bool moving = true;
            while (moving)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, _orignal, LerpSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.localPosition, _orignal ) < 0.1f )
                {
                    moving = false;
                }

                yield return null;
                
            }
        }
    }
}

