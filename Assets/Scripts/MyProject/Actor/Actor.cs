using UnityEngine;

namespace Assets.Scripts.MyProject
{
    public abstract class Actor : MonoBehaviour
    {
        public string Name { get; protected set; }
        public int MaxHp { get; protected set; }
        public int Hp { get; protected set; }
        public int AttackPower { get; protected set; }

        protected ISkill skill;
        protected IDamageCalculator damageCalculator;

        protected void SetActorParameters(string name, int maxHp, int attackPower, ISkill skill, IDamageCalculator damageCalculator)
        {
            Name = name;
            MaxHp = maxHp;
            Hp = MaxHp;
            AttackPower = attackPower;
            this.skill = skill;
            this.damageCalculator = damageCalculator;
        }

        // 공격 메소드
        public virtual void Attack(Actor target)
        {
            int damage = damageCalculator.CalculateDamage(this, target);
            Debug.Log($"{Name}이(가) {target.Name}을(를) 공격하여 {damage}의 데미지를 입혔습니다.");
        }

        // 데미지 입는 메소드
        public virtual void TakeDamage(int damage)
        {
            Hp -= damage;
            Debug.Log($"{Name}이(가) {damage}의 데미지를 입었습니다.");
            if (Hp <= 0)
            {
                Die();
            }
        }

        public virtual void Die()
        {
            Debug.Log($"{Name}이(가) 사망하였습니다.");
        }

        public virtual void UseSkill(Actor target)
        {
            skill.UseSkill(target);
        }

        private void OnTriggerEnter(Collider other)
        {
            TriggerEnter(other);
        }

        private void OnCollisionEnter(Collision collision)
        {
            CollisionEnter(collision);
        }

        protected virtual void TriggerEnter(Collider other)
        {
            
        }

        protected virtual void CollisionEnter(Collision collision)
        {

        }
    }
}
