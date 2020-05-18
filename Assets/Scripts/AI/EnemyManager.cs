using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyScriptObject))]
public class EnemyManager : MonoBehaviour
{
    [Range(1, 100)]
    public int health = 100;

    public EnemyScriptObject enemyPreset;

    private void Start()
    {
        health = enemyPreset.health;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            Destroy(this.gameObject);
    }

    public void damageEnemy(int damage)
    {
        health -= damage;
    }
}
