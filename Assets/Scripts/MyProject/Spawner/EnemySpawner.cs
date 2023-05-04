using UnityEngine;

namespace Assets.Scripts.MyProject
{
    public class EnemySpawner : BaseSpawner
    {
        [Header("Summon")]
        public int SpawnCount;
        public float MinDistance = 10f;
        public float MaxDistance = 15f;
        public float SpawnRadius = 3f;
        public float AllowedDistance = 1.0f;

        private Vector3 prePlyerPosition = Vector3.zero;
        private PlayerActor player  = null;

        private void Update()
        {
            if (player == null)
                player = ActorManager.Instance.GetPlayer();

            if(player != null)
            {
                float distance = Vector3.Distance(prePlyerPosition, player.transform.position);
                if(distance > AllowedDistance)
                {
                    prePlyerPosition = player.transform.position;
                }
            }
        }

        private Vector3 GetRandomPoint()
        {
            // 동서남북 중 랜덤한 방향 선택 (0: 동, 1: 서, 2: 남, 3: 북)
            int direction = Random.Range(0, 4);

            Vector3 randomDirection = Vector3.zero;
            float randomDistance = Random.Range(MinDistance, MaxDistance);

            switch (direction)
            {
                case 0: // 동
                    randomDirection = new Vector3(randomDistance, 0, 0);
                    break;
                case 1: // 서
                    randomDirection = new Vector3(-randomDistance, 0, 0);
                    break;
                case 2: // 남
                    randomDirection = new Vector3(0, 0, -randomDistance);
                    break;
                case 3: // 북
                    randomDirection = new Vector3(0, 0, randomDistance);
                    break;
            }

            Vector3 startPoint = prePlyerPosition + randomDirection;
            Vector3 randomPoint = startPoint + Random.insideUnitSphere * SpawnRadius;

            randomPoint.y = 0f;

            return randomPoint;
        }

        public void SummonMonster()
        {
            int length = Random.Range(3, SpawnCount);
            for(int i = 0; i < length; ++i)
            {
                SpawnObject(0, GetRandomPoint());
            }
        }

        public override GameObject SpawnObject(int prefabIndex, Vector3 position)
        {
            var spawnedObject = base.SpawnObject(prefabIndex, position);
            spawnedObject.name = "Enemy";
            var enemyActor = spawnedObject.GetComponent<EnemyActor>();
            enemyActor.Initialize("몬스터", 1, 10,
                new DefaultSkill(),
                new DamageCalculator());

            ActorManager.Instance.AddEnemy(enemyActor);
            return spawnedObject;
        }
    }
}
