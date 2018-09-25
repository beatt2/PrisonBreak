using UnityEngine;
using UnityEngine.AI;

public class Obstacle : MonoBehaviour
{
    private NavMeshObstacle _meshObstacle;
    private BoxCollider _boxCollider;

    private void Awake()
    {
        _meshObstacle = GetComponent<NavMeshObstacle>();
        _boxCollider = GetComponent<BoxCollider>();
        _meshObstacle.size = _boxCollider.size;
        _meshObstacle.center = _boxCollider.center;
    }
}
