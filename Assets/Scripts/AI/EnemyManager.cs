using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyScriptObject))]
public class EnemyManager : MonoBehaviour
{
    [Range(1, 100)]
    public int health = 100;

    public EnemyScriptObject enemyPreset;

    public AudioClip hitSound;

    private AudioSource source;

    public GameObject explosion;

    private MeleeAI ai;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        ai = GetComponent<MeleeAI>();
    }

    private void Start()
    {
        health = enemyPreset.health;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            
            Destroy(this.gameObject);
        }
            
    }

    public void damageEnemy(int damage)
    {
        health -= damage;
        if (ai.fsm.GetCurrentStateName() != "AlertState")
        {
            ai.fsm.SendEvent("PlayerDetect");
        }
        source.clip = hitSound;
        source.Play();
    }
}
