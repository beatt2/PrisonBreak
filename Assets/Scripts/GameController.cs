using Player;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
    [HideInInspector] public PlayerController Player;

    public Text HintCollect;

    protected override void Awake()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        base.Awake();
    }

    public Transform GetPlayerLocation => Player.transform;

    public void ToggleHint()
    {
        HintCollect.gameObject.SetActive(true);
    }
}

