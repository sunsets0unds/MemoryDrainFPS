using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UnityEngine.AI;

public class AlertAction : FSMAction
{
    private DetectPlayer awareCollider;
    private NavMeshAgent navMeshAgent;
    private Vector3 positionTo;
    private string finishEvent;

    public AlertAction(FSMState owner) : base(owner)
    { }

    public void Init(DetectPlayer trigger, NavMeshAgent navMeshAgent, Vector3 to, string finishEvent = null)
    {
        this.awareCollider = trigger;
        this.navMeshAgent = navMeshAgent;
        this.positionTo = to;
        this.finishEvent = finishEvent;
    }

    public override void OnEnter()
    {
        if (!CheckTrigger())
        {
            Finish();
            return;
        }

        SetPosition(positionTo);
    }

    public override void OnUpdate()
    {
        if (!CheckTrigger())
        {
            Finish();
            return;
        }

        SetPosition(awareCollider.findPlayerInScene());

    }

    private void Finish()
    {
        if (!string.IsNullOrEmpty(finishEvent))
        {
            GetOwner().SendEvent(finishEvent);
        }
    }

    private void SetPosition(Vector3 position)
    {
        navMeshAgent.SetDestination(position);
    }

    private bool CheckTrigger()
    {
        if (awareCollider.playerFound)
            return true;
        else
            return false;
    }
}
