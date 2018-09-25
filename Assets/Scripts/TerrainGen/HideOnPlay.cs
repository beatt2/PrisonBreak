using UnityEngine;

namespace TerrainGen
{
    public class HideOnPlay : MonoBehaviour   
    {
        private void Start()
        {
            gameObject.SetActive(false);
        }
    }
}
