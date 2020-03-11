using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UnityEngine.AI;

public class MoveAction : FSMAction
{
    private Transform transform;
    private NavMeshAgent navMeshAgent;
    private Vector3 positionTo;
    private float duration;
    private float cachedDuration;
    private string finishEvent;

    public MoveAction(FSMState owner) : base (owner)
    { }

    public void Init(Transform transform, NavMeshAgent navMeshAgent, Vector3 to,  float duration, string finishEvent = null)
    {
        this.transform = transform;
        this.navMeshAgent = navMeshAgent;
        this.positionTo = to;
        this.duration = duration;
        this.cachedDuration = duration;
        this.finishEvent = finishEvent;
    }

    public override void OnEnter()
    {
        if(duration <= 0)
        {
            Finish();
            return;
        }

        SetPosition(this.positionTo);
    }

    public override void OnUpdate()
    {
        duration -= Time.deltaTime;

        if(duration <= 0)
        {
            Finish();
            return;
        }

        if(transform.position.x == positionTo.x && transform.position.z == positionTo.z)
        {
            Finish();
            return;
        }
    }

    private void Finish()
    {
        if(!string.IsNullOrEmpty(finishEvent))
        {
            GetOwner().SendEvent(finishEvent);
        }

        duration = cachedDuration;
    }

    private void SetPosition(Vector3 position)
    {
        navMeshAgent.SetDestination(position);
    }
}
