using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UnityEngine.AI;

public class WanderAction : FSMAction
{
    private Transform transform;
    private Vector3 home;
    private NavMeshAgent navMeshAgent;
    private float duration;
    private float cachedDuration;
    private float wanderRadius;
    private string finishEvent;

    public WanderAction(FSMState owner) : base(owner)
    { }

    public void Init(Transform transform, Vector3 home, NavMeshAgent navMeshAgent, float wanderRadius, float duration, string finishEvent = null)
    {
        this.transform = transform;
        this.home = home;
        this.navMeshAgent = navMeshAgent;
        this.wanderRadius = wanderRadius;
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

        if (navMeshAgent.isStopped)
            navMeshAgent.isStopped = false;

        navMeshAgent.SetPath(CalcNextPos(wanderRadius));
    }

    public override void OnUpdate()
    {
        duration -= Time.deltaTime;

        if(duration <= 0)
        {
            Finish();
            return;
        }

        if (navMeshAgent.remainingDistance <= 1)
            navMeshAgent.SetPath(CalcNextPos(wanderRadius));
    }

    private void Finish()
    {
        if (!string.IsNullOrEmpty(finishEvent))
        {
            GetOwner().SendEvent(finishEvent);
        }

        navMeshAgent.isStopped = true;

        duration = cachedDuration;
    }

    private NavMeshPath CalcNextPos(float wanderRadius)
    {
        bool validPos = false;

        do
        {
            Vector3 newPos = home + Random.insideUnitSphere * wanderRadius;

            newPos.y = home.y;

            NavMeshPath newPath = new NavMeshPath();

            navMeshAgent.CalculatePath(newPos, newPath);

            if (newPath.status == NavMeshPathStatus.PathPartial)
            {
                validPos = false;
            }
            else
            {
                validPos = true;
                return newPath;
            }

        } while (!validPos);

        return null;
    }
}
