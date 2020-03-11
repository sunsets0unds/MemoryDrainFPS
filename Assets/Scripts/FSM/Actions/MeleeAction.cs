using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class MeleeAction : FSMAction
{
    private Transform transform;
    private float damage;
    private DetectPlayer trigger;
    private PlayerManager playerManager;
    private string finishEvent;

    public MeleeAction(FSMState owner) : base(owner)
    { }

    public void Init(Transform transform, int damage, DetectPlayer trigger, PlayerManager player, string finishEvent = null)
    {
        this.transform = transform;
        this.damage = damage;
        this.trigger = trigger;
        this.playerManager = player;
        this.finishEvent = finishEvent;
    }

    public override void OnEnter()
    {
        if (Vector3.Distance(transform.position, trigger.findPlayerInScene()) > 2)
        {
            Finish();
            return;
        }
    }

    public override void OnUpdate()
    {
        if (Vector3.Distance(transform.position, trigger.findPlayerInScene()) > 2)
        {
            Finish();
            return;
        }

        playerManager.playerDamage(damage * Time.deltaTime);
    }

    private void Finish()
    {
        if (!string.IsNullOrEmpty(finishEvent))
        {
            GetOwner().SendEvent(finishEvent);
        }
    }
}
