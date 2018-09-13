using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    //TODO: make it so that you can only progress when you killed all enemies

    //TODO: this \/
    ///Go in surround state ---> walk to a point in a radius around player
    /// in handler choose a specific amount of enemies that will attack player
    ///those will walk within reach of player and attack, then retreat or attack again.
    ///handler will then choose other attackers

    [Tooltip("The maximum amount of enemies that will attack at a time")]
    public int MaxAttackers = 2;

    private List<GameObject> _enemies = new List<GameObject>();
    private List<GameObject> _readyToAttack = new List<GameObject>();

    private bool _firstTime = true;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (_firstTime)
        {
            UpdateAttackers();
            _firstTime = false;
        }
    }

    public static EnemyHandler Instance { get; private set; }


    public void EnemySpawned(GameObject pEnemy)
    {
        if (!_enemies.Contains(pEnemy))
        {
            _enemies.Add(pEnemy);
        }
    }

    public void EnemyDied(GameObject pEnemy)
    {
        pEnemy.GetComponent<EnemyMovement>().AddAvailableDegree();

        if (_enemies.Contains(pEnemy))
        {
            _enemies.Remove(pEnemy);
        }

        if (_readyToAttack.Contains(pEnemy))
        {
            _readyToAttack.Remove(pEnemy);
        }

        Destroy(pEnemy);
    }

    public bool IsAttacker(GameObject pEnemy)
    {
        return false;
    }

    public void Ready(GameObject pEnemy)
    {
        _readyToAttack.Add(pEnemy);
    }


    //UNDO UNTIL HERE
    /**/
    public void UpdateAttackers(GameObject pEnemy = null)
    {
        ///remove pEnemy from _attackers
        ///loop over enemies, ignore those in _attackers
        ///store available enemies
        ///choose [MaxAttackers] enemies from available enemies in a predictable way
        ///add these to _attackers


    }
    /**/
}
