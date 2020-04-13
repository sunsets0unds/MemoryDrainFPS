using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemyScriptObject", menuName = "ScriptableObjects/EnemyScriptObject", order = 1)]
public class EnemyScriptObject : ScriptableObject
{
    public int health;
    public int damage;
    public float moveTime;
    public float idleTimeMin;
    public float idleTimeMax;
    public float detectionRadius;
    public float wanderRadius;
}
