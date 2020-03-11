using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class AITest : MonoBehaviour
{
    private FSM fsm;
    private FSMState PatrolState;
    private FSMState IdleState;
    private TextAction PatrolAction;
    private TextAction IdleAction;

    private void Start()
    {
        fsm = new FSM("AITest FSM");
        IdleState = fsm.AddState("IdleState");
        PatrolState = fsm.AddState("PatrolState");
        PatrolAction = new TextAction(PatrolState);
        IdleAction = new TextAction(IdleState);

        PatrolState.AddAction(PatrolAction);
        IdleState.AddAction(IdleAction);

        PatrolState.AddTransition("ToIdle", IdleState);
        IdleState.AddTransition("ToPatrol", PatrolState);

        PatrolAction.Init("AI on patrol", 3f, "ToIdle");
        IdleAction.Init("AI on idle", 2f, "ToPatrol");

        fsm.Start("IdleState");
    }

    private void Update()
    {
        fsm.Update();
    }
}
