using System.Collections;
using UnityEngine;

namespace Items
{
    public class ItemMovement : MonoBehaviour
    {
        private const float Speed = 0.02f;
        private const float Height = 0.02f;

        private Vector3 _targetOne;
        private Vector3 _targetTwo;

        private void Start()
        {
            StartCoroutine(UpDown());
        }

        private IEnumerator UpDown()
        {
            float f = 0;
            _targetOne = new Vector3(transform.position.x, transform.position.y + Height, transform.position.z);
            _targetTwo = new Vector3(transform.position.x, transform.position.y - Height, transform.position.z);

            while (true)
            {
                while (f < 1)
                {
                    f += Time.deltaTime * Speed;
                    transform.position = Vector3.Lerp(_targetOne, _targetTwo, f);
                    transform.Rotate(0, Speed, 0);
                    yield return null;
                }

                while (f > 0)
                {
                    f -= Time.deltaTime * Speed;
                    transform.position = Vector3.Lerp(_targetOne, _targetTwo, f);
                    transform.Rotate(0, Speed, 0);
                    yield return null;
                }
                yield return null;
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}