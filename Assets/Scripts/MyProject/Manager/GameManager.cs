using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.MyProject
{
    public class GameManager : MonoBehaviour
    {
        private EnemySpawner enemySpawner = null;

        private void Start()
        {
            enemySpawner = ObjectPoolManager.Instance.GetSpawner<EnemySpawner>();

            StartCoroutine(SummonMonsterCO());
        }

        IEnumerator SummonMonsterCO()
        {
            yield return new WaitForSeconds(1f);

            while (true) 
            {
                if (enemySpawner != null)
                {
                    int enemtCount = ActorManager.Instance.GetEnemyCount();
                    if (enemtCount <= 0)
                    {
                        enemySpawner.SummonMonster();
                    }

                    yield return null;
                }
            }
        }

        private void OnEnable()
        {
            Events.PlayerDeath += PlayerDied;
        }

        private void OnDisable()
        {
            Events.PlayerDeath -= PlayerDied;
        }

        private void PlayerDied()
        {
            StartCoroutine(RestartGameAfterDelay(0f));
        }

        private IEnumerator RestartGameAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene((int)EScene.Playground);
        }
    }
}
