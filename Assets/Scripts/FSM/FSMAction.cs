using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    /*
     * Written by Michael Amatucci
     * Using tutorial from theknightsofunity.com
     */

    public class FSMAction
    {
        private readonly FSMState owner;

        public FSMAction(FSMState owner)
        {
            this.owner = owner;
        }

        public FSMState GetOwner()
        {
            return owner;
        }

        public virtual void OnEnter()
        { }

        public virtual void OnUpdate()
        { }

        public virtual void OnExit()
        { }

    }

}

