using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.MyProject
{
    public class StateManager
    {
        private Dictionary<Type, IState> stateDictionary;
        private IState currentState;
        private NavMeshAgent agent;
        private Animator animator;
        private AnimID animID;

        public StateManager(Animator animator, NavMeshAgent agent)
        {
            this.agent = agent;
            this.animator = animator;
            stateDictionary = new Dictionary<Type, IState>();
            animID = new AnimID();
        }

        public void RegisterState(IState state)
        {
            if (state is IAnimatedState animatedState)
            {
                animatedState.SetAnimator(animator, animID);
            }

            if (state is IMovableState movableState)
            {
                movableState.SetNavMeshAgent(agent);
            }

            stateDictionary[state.GetType()] = state;
        }

        public void ChangeState<T>() where T : IState
        {
            if (currentState != null)
            {
                currentState.Exit();
            }

            currentState = stateDictionary[typeof(T)];
            currentState.Enter();
        }

        public void Update()
        {
            currentState.Execute();
        }
    }
}
