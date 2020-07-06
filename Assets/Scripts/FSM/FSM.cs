using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    /*
     * Written by Michael Amatucci
     * Using tutorial from theknightsofunity.com
     */

    public class FSM
    {
        private readonly string name;
        private FSMState currentState;
        private readonly Dictionary<string, FSMState> stateMap;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public FSM(string name)
        {
            this.name = name;
            this.currentState = null;
            stateMap = new Dictionary<string, FSMState>();
        }

        public void Start(string stateName)
        {
            if (!stateMap.ContainsKey(stateName))
            {
                Debug.LogWarning("This FSM doesn't contain: " + stateName);
                return;
            }

            ChangeToState(stateMap[stateName]);
        }

        public void ChangeToState(FSMState state)
        {
            if (this.currentState != null)
            {
                ExitState(this.currentState);
            }

            this.currentState = state;
            EnterState(this.currentState);
        }

        public void EnterState(FSMState state)
        {
            ProcessStateAction(state, delegate (FSMAction action)
            {
                action.OnEnter();
            });
        }

        public void ExitState(FSMState state)
        {
            FSMState currentStateOnInvoke = this.currentState;

            ProcessStateAction(state, delegate (FSMAction action)
            {
                if (this.currentState != currentStateOnInvoke)
                    Debug.LogError("State cannot be changed on exit of the specified state");

                action.OnExit();
            });
        }

        public void Update()
        {
            if (this.currentState == null)
                return;

            ProcessStateAction(this.currentState, delegate (FSMAction action)
            {
                action.OnUpdate();
            });
        }

        private delegate void StateActionProcessor(FSMAction action);

        private void ProcessStateAction(FSMState state, StateActionProcessor actionProcessor)
        {
            FSMState currentStateOnInvoke = this.currentState;
            IEnumerable<FSMAction> actions = state.GetActions();

            foreach (FSMAction action in actions)
            {
                if (this.currentState != currentStateOnInvoke)
                {
                    break;
                }

                actionProcessor(action);
            }
        }

        public void SendEvent(string eventId)
        {
            FSMState transitionState = ResolveTransition(eventId);

            if (transitionState == null)
                Debug.LogWarning("The current state has no transition for event " + eventId);
            else
                ChangeToState(transitionState);
        }

        private FSMState ResolveTransition(string eventId)
        {
            FSMState transitionState = this.currentState.GetTransition(eventId);

            if (transitionState == null)
                return null;
            else
                return transitionState;
        }

        public FSMState AddState(string name)
        {
            if(stateMap.ContainsKey(name))
            {
                Debug.LogWarning("The FSM already contains: " + name);
                return null;
            }

            FSMState newState = new FSMState(name, this);
            stateMap[name] = newState;
            return newState;
        }

        public string GetCurrentStateName()
        {
            return this.currentState.GetName();
        }

    }
}

