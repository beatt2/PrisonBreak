using UnityEditor;
using UnityEngine;

namespace TerrainGen.Data
{
    public class UpdateableData : ScriptableObject
    {
        public event System.Action OnValueUpdated;
        public bool AutoUpdate;

#if UNITY_EDITOR

        protected virtual void OnValidate()
        {
            if (AutoUpdate)
            {
         
                EditorApplication.update += NotifyOfUpdatedValues;
 

            }
        }

        public void NotifyOfUpdatedValues()
        {
            // ReSharper disable once DelegateSubtraction
            EditorApplication.update -= NotifyOfUpdatedValues;

            if (OnValueUpdated == null) return;
            OnValueUpdated();
            EditorUtility.SetDirty(this);
        }
    }
#endif
}
