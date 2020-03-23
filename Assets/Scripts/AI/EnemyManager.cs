using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Range(1, 100)]
    public int health = 100;

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
