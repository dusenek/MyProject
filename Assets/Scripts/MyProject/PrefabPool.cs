using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MyProject
{
    public interface IPoolingStrategy
    {
        GameObject GetFromPool();
        void ReturnToPool(GameObject objectToReturn);
        void SetPrefab(GameObject newPrefab);
        void SetPoolSize(int newSize);
        int GetObjectCount();
        bool Contains(GameObject objectToCheck); // 새로운 메서드를 추가합니다.
    }

    public class PrefabPool : IPoolingStrategy
    {
        private GameObject prefab;
        private int poolSize;
        private Queue<GameObject> objectPool;
        private Transform spawnRoot = null;

        public PrefabPool(GameObject prefab, int poolSize, Transform spawnRoot = null)
        {
            this.prefab = prefab;
            this.poolSize = poolSize;
            this.spawnRoot = spawnRoot;

            InitializePool();
        }

        private void InitializePool()
        {
            objectPool = new Queue<GameObject>(poolSize);

            for (int i = 0; i < poolSize; i++)
            {
                AddObject(prefab);
            }
        }

        private void AddObject(GameObject prefab)
        {
            GameObject poolObject = GameObject.Instantiate(prefab);
            poolObject.SetActive(false);
            if (spawnRoot != null)
                poolObject.transform.SetParent(spawnRoot);
            objectPool.Enqueue(poolObject);
        }

        public GameObject GetFromPool()
        {
            if (objectPool.Count == 0)
            {
                AddObject(prefab);
            }

            GameObject objectFromPool = objectPool.Dequeue();
            objectFromPool.SetActive(true);

            return objectFromPool;
        }

        public void ReturnToPool(GameObject objectToReturn)
        {
            objectToReturn.SetActive(false);
            objectPool.Enqueue(objectToReturn);
        }

        public void SetPrefab(GameObject newPrefab)
        {
            prefab = newPrefab;
        }

        public void SetPoolSize(int newSize)
        {
            if (newSize < poolSize)
            {
                while (objectPool.Count > newSize)
                {
                    GameObject excessObject = objectPool.Dequeue();
                    GameObject.Destroy(excessObject);
                }
            }
            else
            {
                for (int i = poolSize; i < newSize; i++)
                {
                    AddObject(prefab);
                }
            }
            poolSize = newSize;
        }

        public int GetObjectCount()
        {
            return objectPool.Count;
        }

        public bool Contains(GameObject objectToCheck)
        {
            return objectPool.Contains(objectToCheck);
        }
    }
}
