using Assets.Scripts.MyProject;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.MyProject
{
    public class BaseSpawner : MonoBehaviour
    {
        public List<GameObject> objectPrefabs = null;
        public Transform spawnRoot = null;
        public int poolSize;

        private Dictionary<GameObject, IPoolingStrategy> poolingStrategies;

        private void Awake()
        {
            poolingStrategies = new Dictionary<GameObject, IPoolingStrategy>();

            foreach (var prefab in objectPrefabs)
            {
                var strategy = new PrefabPool(prefab, poolSize, spawnRoot);
                poolingStrategies.Add(prefab, strategy);
            }
        }

        public virtual GameObject SpawnObject(int prefabIndex, Vector3 position)
        {
            if (!IsValidPrefabIndex(prefabIndex))
            {
                Debug.LogError($"Invalid prefab index: {prefabIndex}");
                return null;
            }
            
            GameObject prefab = objectPrefabs[prefabIndex];
            GameObject spawnedObject = poolingStrategies[prefab].GetFromPool();
            spawnedObject.transform.position = position;
            return spawnedObject;
        }

        public virtual void DespawnObject(int prefabIndex, GameObject objectToDespawn)
        {
            if (!IsValidPrefabIndex(prefabIndex))
            {
                Debug.LogError($"Invalid prefab index: {prefabIndex}");
                return;
            }

            GameObject prefab = objectPrefabs[prefabIndex];
            poolingStrategies[prefab].ReturnToPool(objectToDespawn);
        }

        private bool IsValidPrefabIndex(int index)
        {
            return index >= 0 && index < objectPrefabs.Count;
        }
    }
}