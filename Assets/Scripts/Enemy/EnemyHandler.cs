using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    //TODO: make it so that you can only progress when you killed all enemies

    [Tooltip("The maximum amount of enemies that will attack at a time")]
    public int MaxAttackers = 2;

    private static EnemyHandler _handler;

    public List<GameObject> Enemies = new List<GameObject>();

    private bool _firstTime = true;

    // Use this for initialization
    void Awake()
    {
        _handler = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (_firstTime)
        {
            //UpdateAttackers();
            _firstTime = false;
        }
    }

    public static EnemyHandler Instance
    {
        get
        {
            return _handler;
        }
    }

    /**
    public void UpdateAttackers(GameObject pEnemy = null)
    {
        if (pEnemy != null && Attackers.Contains(pEnemy))
        {
            Attackers.Remove(pEnemy);
        }

        if (Attackers.Count >= MaxAttackers || Enemies.Count == 0)
        {
            return;
        }

        List<int> idlers = new List<int>();

        for (int i = 0; i < Enemies.Count; i++)
        {
            GameObject enemy = Enemies[i];
            if (!Attackers.Contains(enemy))
            {
                idlers.Add(i);
            }
        }

        while (Attackers.Count < MaxAttackers)
        {
            if (idlers.Count == 0)
            {
                return;
            }

            int index = Random.Range(0, idlers.Count);
            int enemyIndex = idlers[index];
            idlers.RemoveAt(index);

            GameObject enemy = Enemies[enemyIndex];

            Attackers.Add(enemy);
        }
    }
    /**/
}
