using UnityEngine;

namespace Assets.Scripts.MyProject
{
    // 스킬 인터페이스
    public interface ISkill
    {
        void UseSkill(Actor target);
    }

    public class DefaultSkill : ISkill
    {
        public void UseSkill(Actor target)
        {
            Debug.Log($"{target.Name} 스킬사용");
        }
    }
}
