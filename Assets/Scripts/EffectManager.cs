using Tools;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    public GameObject Explosion;

    public void InstansiateExplosion(Transform yourTransform)
    {
        Instantiate(Explosion, yourTransform.position, yourTransform.rotation);
    }

}
