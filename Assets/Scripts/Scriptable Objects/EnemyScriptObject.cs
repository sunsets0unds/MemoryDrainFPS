using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemyScriptObject", menuName = "ScriptableObjects/EnemyScriptObject", order = 1)]
public class EnemyScriptObject : ScriptableObject
{
    public int health;
    public int damage;
    public float moveTime;
    public float idleTime;
    public float detectionRadius;
}
