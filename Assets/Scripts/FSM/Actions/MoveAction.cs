using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UnityEngine.AI;

public class MoveAction : FSMAction
{
    private Transform transform;
    private NavMeshAgent navMeshAgent;
    private float duration;
    private float cachedDuration;
    private string finishEvent;
    private Vector3[] waypoints;
    private int count = 0;

    public MoveAction(FSMState owner) : base (owner)
    { }

    public void Init(Transform transform, NavMeshAgent navMeshAgent, Vector3[] waypoints, float duration, string finishEvent = null)
    {
        this.transform = transform;
        this.navMeshAgent = navMeshAgent;
        this.duration = duration;
        this.cachedDuration = duration;
        this.waypoints = waypoints;
        this.finishEvent = finishEvent;
        this.count = 0;
    }

    public override void OnEnter()
    {
        if(duration <= 0)
        {
            Finish();
            return;
        }

        if (navMeshAgent.isStopped)
            navMeshAgent.isStopped = false;

        SetPosition(waypoints[count]);
    }

    public override void OnUpdate()
    {
        duration -= Time.deltaTime;

        if(duration <= 0)
        {
            Finish();
            return;
        }

        if(transform.position.x == waypoints[count].x && transform.position.z == waypoints[count].z)
        {
            count++;

            if(count >= waypoints.Length)
            {
                Finish();
                return;
            }

            SetPosition(waypoints[count]);
        }
    }

    private void Finish()
    {
        if(!string.IsNullOrEmpty(finishEvent))
        {
            GetOwner().SendEvent(finishEvent);
        }

        navMeshAgent.isStopped = true;

        duration = cachedDuration;
        if (count >= waypoints.Length)
            count = 0;
    }

    private void SetPosition(Vector3 position)
    {
        navMeshAgent.SetDestination(position);
    }
}
