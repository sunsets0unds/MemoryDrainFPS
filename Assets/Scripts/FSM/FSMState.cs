using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class FSMState
    {
        private List<FSMAction> actions;
        private readonly string name;
        private readonly FSM owner;
        private readonly Dictionary<string, FSMState> transitionMap;

        public FSMState(string name, FSM owner)
        {
            this.name = name;
            this.owner = owner;
            this.transitionMap = new Dictionary<string, FSMState>();
            this.actions = new List<FSMAction>();
        }

        public void AddTransition(string id, FSMState destinationState)
        {
            if(transitionMap.ContainsKey(id))
            {
                Debug.LogError(string.Format("State {0} already contains transition for {1}", this.name, id));
                return;
            }

            transitionMap[id] = destinationState;
        }

        public FSMState GetTransition(string eventId)
        {
            if(transitionMap.ContainsKey(eventId))
            {
                return transitionMap[eventId];
            }

            return null;
        }

        public void AddAction(FSMAction action)
        {
            if(actions.Contains(action))
            {
                Debug.LogWarning("This state already contains " + action);
                return;
            }

            if (action.GetOwner() != this)
            {
                Debug.LogWarning("This state doesn't own " + action);
            }

            actions.Add(action);
        }

        public IEnumerable<FSMAction> GetActions()
        {
            return actions;
        }

        public void SendEvent(string eventId)
        {
            this.owner.SendEvent(eventId);
        }

    }

}

