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
    public float idleTime = 3f;
    public GameObject[] waypoints;
    private Vector3[] waypointPos;

    private Vector3 home;
    private DetectPlayer trigger;
    private NavMeshAgent navMeshAgent;

    private FSM fsm;
    private FSMState MoveState;
    private FSMState IdleState;
    private FSMState AlertState;
    private FSMState MeleeState;

    private MoveAction MoveAction;
    private AlertAction alertAction;
    private MeleeAction meleeAction;
    private TextAction IdleAction;

    // Start is called before the first frame update
    void Awake()
    {
        damage = enemyPreset.damage;
        moveTime = enemyPreset.moveTime;
        idleTime = enemyPreset.idleTime;
        waypointPos = new Vector3[waypoints.Length];

        int i = 0;

        foreach (GameObject w in waypoints)
        {
            waypointPos[i] = w.transform.position;
            i++;
        }

        home = transform.position;

        trigger = GetComponentInChildren<DetectPlayer>();

        navMeshAgent = GetComponent<NavMeshAgent>();

        fsm = new FSM("MeleeAI FSM");

        MoveState = fsm.AddState("MoveState");
        IdleState = fsm.AddState("IdleState");
        AlertState = fsm.AddState("AlertState");
        MeleeState = fsm.AddState("MeleeState");

        MoveAction = new MoveAction(MoveState);
        IdleAction = new TextAction(IdleState);
        alertAction = new AlertAction(AlertState);
        meleeAction = new MeleeAction(MeleeState);

        MoveState.AddAction(MoveAction);
        IdleState.AddAction(IdleAction);
        AlertState.AddAction(alertAction);
        MeleeState.AddAction(meleeAction);

        MoveState.AddTransition("ToIdle", IdleState);
        MoveState.AddTransition("PlayerDetect", AlertState);
        IdleState.AddTransition("ToMove", MoveState);
        IdleState.AddTransition("PlayerDetect", AlertState);

        AlertState.AddTransition("ToIdle", IdleState);
        AlertState.AddTransition("ToMelee", MeleeState);
        MeleeState.AddTransition("ToAlert", AlertState);

        MoveAction.Init(this.transform, navMeshAgent, waypointPos, moveTime, "ToIdle");
        IdleAction.Init("Idling", idleTime, "ToMove");

        alertAction.Init(trigger, navMeshAgent, trigger.findPlayerInScene(), "ToIdle");
        meleeAction.Init(this.transform, damage, trigger, FindObjectOfType<PlayerManager>(), "ToAlert");

        fsm.Start("IdleState");
    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update();

        if(trigger.playerFound)
        {
            fsm.SendEvent("PlayerDetect");
        }

    }

    private void LateUpdate()
    {
        if (Vector3.Distance(transform.position, trigger.findPlayerInScene()) <= 1)
        {
            fsm.SendEvent("ToMelee");
        }
    }
}
