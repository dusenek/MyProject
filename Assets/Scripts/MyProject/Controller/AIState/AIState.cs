using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.MyProject
{
    public interface IState
    {
        void Enter();
        void Execute();
        void Exit();
    }

    public interface IAnimatedState
    {
        void SetAnimator(Animator animator, AnimID animid);
    }

    public interface IMovableState
    {
        void SetNavMeshAgent(NavMeshAgent agent);
    }

    public class StateBase : IAnimatedState
    {
        protected AIController controller;
        protected Animator animator;
        protected AnimID animID;

        public StateBase(AIController controller)
        {
            this.controller = controller;
        }

        public void SetAnimator(Animator animator, AnimID animid)
        {
            this.animator = animator;
            this.animID = animid;
        }
    }

    public class WaitState : StateBase, IState
    {
        public WaitState(AIController controller) : base(controller)
        {
        }

        public void Enter()
        {
            animator.SetBool(animID.Grounded, true);
            animator.SetFloat(animID.Speed, 0);
            animator.SetFloat(animID.MotionSpeed, 1f);
        }

        public void Execute()
        {
            controller.stateManager.ChangeState<ChaseState>();
        }

        public void Exit()
        {
        }
    }

    public class ChaseState : StateBase, IState, IMovableState
    {
        private NavMeshAgent agent;
        private GameObject player; 

        public ChaseState(AIController controller) : base(controller)
        {
        }

        public void SetNavMeshAgent(NavMeshAgent agent)
        {
            this.agent = agent;
        }

        public void Enter()
        {
            if (animator != null)
            {
                animator.SetFloat(animID.Speed, 6f);
                animator.SetFloat(animID.MotionSpeed, 1f);
            }
        }

        public void Execute()
        {
            if(player == null)
            {
                var tempPlayer = ActorManager.Instance.GetPlayer();
                if (tempPlayer != null)
                {
                    player = tempPlayer.gameObject;
                }
            }

            if(player != null)
            {
                agent.SetDestination(player.transform.position);
            }
        }

        public void Exit()
        {
        }
    }

    public class DeadState : StateBase, IState
    {
        public DeadState(AIController controller) : base(controller)
        {
        }

        public void Enter()
        {
            controller.stateManager.ChangeState<WaitState>();
        }

        public void Execute()
        {
        }

        public void Exit()
        {
        }
    }
}
