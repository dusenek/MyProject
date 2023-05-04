using System;
using UnityEngine;

namespace Assets.Scripts.MyProject
{
    public class EnemyActor : Actor
    {
        public event Action onDeath;

        private bool isInit = false;

        private void Start()
        {
            isInit = true;
        }

        public void Initialize(string name, int maxHp, int attackPower, ISkill skill, IDamageCalculator damageCalculator)
        {
            base.SetActorParameters(name, maxHp, attackPower, skill, damageCalculator);
        }

        protected override void TriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && isInit)
            {
                var player = ActorManager.Instance.GetPlayer();
                if (player != null)
                {
                    Attack(player);
                    // 공격하고 바로죽는다.
                    Attack(this);
                }
            }
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);

            if (Hp <= 0)
            {
                onDeath?.Invoke();

                ActorManager.Instance.RemoveEnemy(this);
            }
        }
    }
}
