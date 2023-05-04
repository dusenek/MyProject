using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MyProject
{
    public class ObjectPoolManager : MonoBehaviour
    {
        public static ObjectPoolManager Instance { get; private set; }

        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private PlaySpawner playSpawner;
        [SerializeField] private ProjectileSpawner projectileSpawner;
        [SerializeField] private BuffSpawner buffSpawner;

        private Dictionary<Type, BaseSpawner> spawners;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            spawners = new Dictionary<Type, BaseSpawner>
            {
                { typeof(EnemySpawner), enemySpawner },
                { typeof(PlaySpawner), playSpawner },
                { typeof(ProjectileSpawner), projectileSpawner },
                { typeof(BuffSpawner), buffSpawner }
            };
        }

        public T GetSpawner<T>() where T : BaseSpawner
        {
            if (spawners.TryGetValue(typeof(T), out BaseSpawner spawner))
            {
                return spawner as T;
            }

            return null;
        }
    }
}
