using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(EnemyScriptObject))]
public class MeleeAI : MonoBehaviour
{
    public EnemyScriptObject enemyPreset;
    [Range(1, 20)]
    public int damage = 1;
    [Range(0f, 10f)]
    public float moveTime = 1f;
    [Range(0f, 10f)]
    public float idleTimeMin = 3f;
    [Range(5f, 20f)]
    public float idleTimeMax = 5f;
    [Range(1f, 20f)]
    public float wanderRadius = 5f;

    private Vector3 home;
    private DetectPlayer trigger;
    private NavMeshAgent navMeshAgent;

    private FSM fsm;
    private FSMState WanderState;
    private FSMState IdleState;
    private FSMState AlertState;
    private FSMState MeleeState;

    private WanderAction WanderAction;
    private AlertAction alertAction;
    private MeleeAction meleeAction;
    private TextAction IdleAction;

    // Start is called before the first frame update
    void Awake()
    {
        damage = enemyPreset.damage;
        moveTime = enemyPreset.moveTime;
        idleTimeMin = enemyPreset.idleTimeMin;
        idleTimeMax = enemyPreset.idleTimeMax;
        wanderRadius = enemyPreset.wanderRadius;

        home = transform.position;

        trigger = GetComponentInChildren<DetectPlayer>();

        trigger.GetComponent<SphereCollider>().radius = enemyPreset.detectionRadius;

        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.speed = enemyPreset.speed;

        fsm = new FSM("MeleeAI FSM");

        WanderState = fsm.AddState("WanderState");
        IdleState = fsm.AddState("IdleState");
        AlertState = fsm.AddState("AlertState");
        MeleeState = fsm.AddState("MeleeState");

        WanderAction = new WanderAction(WanderState);
        IdleAction = new TextAction(IdleState);
        alertAction = new AlertAction(AlertState);
        meleeAction = new MeleeAction(MeleeState);

        WanderState.AddAction(WanderAction);
        IdleState.AddAction(IdleAction);
        AlertState.AddAction(alertAction);
        MeleeState.AddAction(meleeAction);

        WanderState.AddTransition("ToIdle", IdleState);
        WanderState.AddTransition("PlayerDetect", AlertState);
        IdleState.AddTransition("ToWander", WanderState);
        IdleState.AddTransition("PlayerDetect", AlertState);

        AlertState.AddTransition("ToIdle", IdleState);
        AlertState.AddTransition("ToMelee", MeleeState);
        MeleeState.AddTransition("ToAlert", AlertState);

        WanderAction.Init(this.transform, home, navMeshAgent, wanderRadius, moveTime, "ToIdle");
        IdleAction.Init("Idling", Random.Range(idleTimeMin, idleTimeMax), "ToWander");

        alertAction.Init(trigger, navMeshAgent, trigger.findPlayerInScene(), "ToIdle");
        meleeAction.Init(this.transform, damage, trigger, FindObjectOfType<PlayerManager>(), "ToAlert");

        fsm.Start("IdleState");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update();

        if(trigger.playerFound)
        {
            if(fsm.GetCurrentStateName() != "AlertState")
            {
                fsm.SendEvent("PlayerDetect");
            }
        }

    }

    private void LateUpdate()
    {
        if (Vector3.Distance(transform.position, trigger.findPlayerInScene()) <= 1)
        {
            if(fsm.GetCurrentStateName() == "AlertState")
            {
                fsm.SendEvent("ToMelee");
            }
        }
    }
}
