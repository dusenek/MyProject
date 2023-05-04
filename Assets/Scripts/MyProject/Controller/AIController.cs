using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.MyProject
{
    public class AIController : MonoBehaviour
    {
        public StateManager stateManager;
        public Animator animator; 
        public NavMeshAgent navMeshAgent;
        public EnemyActor enemyActor;

        void Start()
        {
            stateManager = new StateManager(animator, navMeshAgent);

            stateManager.RegisterState(new WaitState(this));
            stateManager.RegisterState(new ChaseState(this));
            stateManager.RegisterState(new DeadState(this));

            stateManager.ChangeState<WaitState>();

            enemyActor.onDeath += HandleDeath;
        }

        void Update()
        {
            stateManager.Update();
        }

        private void HandleDeath()
        {
            stateManager.ChangeState<DeadState>(); // 죽는 상태로 전환
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
        }

        private void OnLand(AnimationEvent animationEvent)
        {
        }
    }
}
