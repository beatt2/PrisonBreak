using System.Linq;
using TerrainGen.Data;
using UnityEngine;

namespace TerrainGen
{
    public class ObjectSpawner : MonoBehaviour
    {
        public ObjectSpawnData [] ObjectData;
        public GameObject Parent;


        public void SpawnObjects(MeshData meshData)
        {

            var tempChildsList = Parent.transform.Cast<Transform>().ToList();
            foreach (var child in tempChildsList)
            {
    
                DestroyImmediate(child.gameObject);
            }

            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < ObjectData.Length; i++)
            {
                var tempList = meshData.GetVertices(ObjectData[i]);
                for (int j = 0; j < tempList.Count; j++)
                {
                    float xRandom = Random.Range(-0.5f, 0.5f);
                    float zRandom = Random.Range(-0.5f, 0.5f);
                    Vector3 target = new Vector3(tempList[j].x + xRandom, tempList[j].y + 5, tempList[j].z + zRandom);

                    GameObject temp = Instantiate(ObjectData[i].ObjectToSpawn, target, ObjectData[i].ObjectToSpawn.transform.rotation, Parent.transform);

                    RaycastHit hit;
                    Ray myRay = new Ray(temp.transform.position, -temp.transform.up);
                    if (Physics.Raycast(myRay, out hit, 10))
                    {
                        float height = hit.point.y;
                        temp.transform.position = new Vector3(
                            temp.transform.position.x,
                            height,
                            temp.transform.position.z
                        );
                    }

                }
            }
        }



    }
}
