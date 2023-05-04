using UnityEngine;

namespace Assets.Scripts.MyProject
{
    // 데미지 계산기 인터페이스
    public interface IDamageCalculator
    {
        int CalculateDamage(Actor attacker, Actor target);
    }

    // 데미지 계산기 구현 클래스
    public class DamageCalculator : IDamageCalculator
    {
        public int CalculateDamage(Actor attacker, Actor target)
        {
            int damage = Random.Range((int)(attacker.AttackPower * 0.5f), attacker.AttackPower);
            target.TakeDamage(damage);
            return damage;
        }
    }
}
