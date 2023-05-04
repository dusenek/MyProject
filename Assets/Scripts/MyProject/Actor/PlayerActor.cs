using StarterAssets;
using UnityEngine;
namespace Assets.Scripts.MyProject
{
    public class PlayerActor : Actor
    {
        private void Start()
        {
            // 조이스틱 연결
            var ui = GameObject.Find("UI_Canvas_StarterAssetsInputs_Joysticks");
            ui.GetComponent<UICanvasControllerInput>().starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        }

        public void Initialize(string name, int maxHp, int attackPower, ISkill skill, IDamageCalculator damageCalculator)
        {
            base.SetActorParameters(name, maxHp, attackPower, skill, damageCalculator);
        }

        public override void Die()
        {
            base.Die();

            GameOver();
        }

        private void GameOver()
        {
            Events.OnPlayerDie();
        }
    }
}
