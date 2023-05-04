using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MyProject
{
    public class ActorManager : MonoBehaviour
    {
        private static ActorManager instance = null;
        public static ActorManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("ActorManager");
                    instance = singletonObject.AddComponent<ActorManager>();
                    DontDestroyOnLoad(singletonObject);
                }

                return instance;
            }
        }

        public EnemySpawner spawner;

        // Actor Lists
        private PlayerActor player = null;  // 플레이어가 하나
        private Dictionary<int, EnemyActor> enemies = new Dictionary<int, EnemyActor>();

        private void Awake()
        {
            instance = this;
        }

        public PlayerActor GetPlayer()
        {
            return player;
        }

        public void AddPlayer(PlayerActor newPlayer)
        {
            player = newPlayer;
        }

        public void AddEnemy(EnemyActor newEnemy)
        {
            enemies.Add(newEnemy.gameObject.GetHashCode(), newEnemy);
        }

        public void RemovePlayer()
        {
            player = null;
        }

        public void RemoveEnemy(EnemyActor enemy)
        {
            if (enemies.ContainsValue(enemy))
            {
                enemies.Remove(enemy.gameObject.GetHashCode());
                spawner.DespawnObject(0, enemy.gameObject);
            }
        }

        public EnemyActor GetEnemyByKey(int key)
        {
            if (enemies.ContainsKey(key) == false)
            {
                return null;
            }

            return enemies[key];
        }

        public int GetEnemyCount()
        {
            return enemies.Count;
        }
    }

}
