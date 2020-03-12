using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MeleeAI : MonoBehaviour
{
    [Range(1, 20)]
    public int damage = 1;
    [Range(0f, 10f)]
    public float moveTime = 1f;
    [Range(0f, 10f)]
    public float idleTime = 3f;
    public Vector3 pos1;
    public Vector3 pos2;

    private Vector3 home;
    private DetectPlayer trigger;
    private NavMeshAgent navMeshAgent;

    private FSM fsm;
    private FSMState MoveLeftState;
    private FSMState MoveRightState;
    private FSMState IdleState1;
    private FSMState IdleState2;
    private FSMState AlertState;
    private FSMState MeleeState;

    private MoveAction MoveLeftAction;
    private MoveAction MoveRightAction;
    private AlertAction alertAction;
    private MeleeAction meleeAction;
    private TextAction IdleAction1;
    private TextAction IdleAction2;

    [SerializeField]
    private string currentState;

    // Start is called before the first frame update
    void Start()
    {
        home = transform.position;

        trigger = GetComponentInChildren<DetectPlayer>();

        navMeshAgent = GetComponent<NavMeshAgent>();

        fsm = new FSM("AIMoveTest FSM");
        MoveLeftState = fsm.AddState("MoveLeftState");
        MoveRightState = fsm.AddState("MoveRightState");
        IdleState1 = fsm.AddState("IdleState1");
        IdleState2 = fsm.AddState("IdleState2");
        AlertState = fsm.AddState("AlertState");
        MeleeState = fsm.AddState("MeleeState");

        MoveLeftAction = new MoveAction(MoveLeftState);
        MoveRightAction = new MoveAction(MoveRightState);
        IdleAction1 = new TextAction(IdleState1);
        IdleAction2 = new TextAction(IdleState2);
        alertAction = new AlertAction(AlertState);
        meleeAction = new MeleeAction(MeleeState);

        MoveLeftState.AddAction(MoveLeftAction);
        MoveRightState.AddAction(MoveRightAction);
        IdleState1.AddAction(IdleAction1);
        IdleState2.AddAction(IdleAction2);
        AlertState.AddAction(alertAction);
        MeleeState.AddAction(meleeAction);

        MoveLeftState.AddTransition("ToIdle2", IdleState2);
        MoveLeftState.AddTransition("PlayerDetect", AlertState);
        IdleState2.AddTransition("ToRight", MoveRightState);
        IdleState2.AddTransition("PlayerDetect", AlertState);
        MoveRightState.AddTransition("ToIdle1", IdleState1);
        MoveRightState.AddTransition("PlayerDetect", AlertState);
        IdleState1.AddTransition("ToLeft", MoveLeftState);
        IdleState1.AddTransition("PlayerDetect", AlertState);

        AlertState.AddTransition("ToIdle1", IdleState1);
        AlertState.AddTransition("ToMelee", MeleeState);
        MeleeState.AddTransition("ToAlert", AlertState);

        MoveLeftAction.Init(this.transform, navMeshAgent, pos1, moveTime, "ToIdle2");
        IdleAction2.Init("Idling", idleTime, "ToRight");
        MoveRightAction.Init(this.transform, navMeshAgent, pos2, moveTime, "ToIdle1");
        IdleAction1.Init("Idling", idleTime, "ToLeft");

        alertAction.Init(trigger, navMeshAgent, trigger.findPlayerInScene(), "ToIdle1");
        meleeAction.Init(this.transform, damage, trigger, FindObjectOfType<PlayerManager>(), "ToAlert");

        fsm.Start("IdleState1");
    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update();

        if(trigger.playerFound)
        {
            fsm.SendEvent("PlayerDetect");
        }

        if(Vector3.Distance(transform.position, trigger.findPlayerInScene()) <= 1)
        {
            fsm.SendEvent("ToMelee");
        }

        currentState = fsm.GetCurrentStateName();
    }
}
