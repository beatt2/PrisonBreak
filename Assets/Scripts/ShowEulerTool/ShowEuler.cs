using UnityEngine;

namespace ShowEulerTool
{
   [ExecuteInEditMode]
   public class ShowEuler : MonoBehaviour
   {
      public Vector3 EulerAngles = Vector3.zero;



      private void Awake()
      {
         EulerAngles = transform.eulerAngles;

      }

      private void Update()
      {

         EulerAngles = transform.eulerAngles;

      }
   }
}
