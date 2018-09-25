using System.Collections;
using System.Collections.Generic;
using TerrainGen;
using TerrainGen.Data;
using UnityEngine;

[CreateAssetMenu]
public class BaseSpawnSettings : UpdateableData
{
    public float StartHeight;
    public bool UseBaseSpawn;

    public GameObject go;

    [HideInInspector]
    public Vector3 SpawnLocation = new Vector3();

    public void SetSpawnLocation(Vector3 location)
    {
       SpawnLocation = GritSpace.GritToWorld(location);
    }

}
