using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UnityEngine.AI;

public class AlertAction : FSMAction
{
    private DetectPlayer awareCollider;
    private NavMeshAgent navMeshAgent;
    private string finishEvent;

    public AlertAction(FSMState owner) : base(owner)
    { }

    public void Init(DetectPlayer trigger, NavMeshAgent navMeshAgent, string finishEvent = null)
    {
        this.awareCollider = trigger;
        this.navMeshAgent = navMeshAgent;
        this.finishEvent = finishEvent;
    }

    public override void OnEnter()
    {
        SetPosition(awareCollider.findPlayerInScene());

        if (!CheckTrigger())
        {
            Finish();
            return;
        }
        
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
