using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.MyProject
{
    public class BuffSpawner : BaseSpawner
    {
        public float spawnInterval = 5f;
        public float spawnRadius = 10f;
        public int maxBuffItems = 5; 

        private int currentBuffItems = 0;
        private Transform playerTransform;

        private void OnEnable()
        {
            Events.PlayerCreated += OnPlayerCreated;
        }

        private void OnDisable()
        {
            Events.PlayerCreated -= OnPlayerCreated;
        }

        private void Start()
        {
            StartCoroutine(SpawnBuffItemsCO());
        }

        private void OnPlayerCreated(GameObject playerGameObject)
        {
            playerTransform = playerGameObject.transform;
        }

        public override GameObject SpawnObject(int prefabIndex, Vector3 position)
        {
            if (currentBuffItems >= maxBuffItems)
                return null;

            var spawnedObject = base.SpawnObject(prefabIndex, position);
            spawnedObject.name = "Buff";
            spawnedObject.GetComponentInChildren<BuffItem>().Initialize(this);

            currentBuffItems++;
            return spawnedObject;
        }

        private IEnumerator SpawnBuffItemsCO()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnInterval);

                if (currentBuffItems < maxBuffItems)
                {
                    Vector2 randomPosition = Random.insideUnitCircle * spawnRadius;
                    Vector3 spawnPosition = new Vector3(playerTransform.position.x + randomPosition.x, 1f, playerTransform.position.y + randomPosition.y);

                    SpawnObject(0, spawnPosition);
                }
            }
        }
    }
}
